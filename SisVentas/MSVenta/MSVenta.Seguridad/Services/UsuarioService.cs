using Microsoft.EntityFrameworkCore;
using MSVenta.Seguridad.DTOs;
using MSVenta.Seguridad.Models;
using MSVenta.Seguridad.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSVenta.Seguridad.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ContextDatabase _context;

        public UsuarioService(ContextDatabase context)
        {
            _context = context;
        }


        public async Task<IEnumerable<UsuarioDTO>> GetAllUsuarios()
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.RolPermisoUsuarios)
                    .ThenInclude(rpu => rpu.RolPermiso)
                        .ThenInclude(rp => rp.Rol)
                .Include(u => u.RolPermisoUsuarios)
                    .ThenInclude(rpu => rpu.RolPermiso)
                        .ThenInclude(rp => rp.Permiso)
                .ToListAsync();

            return usuarios.Select(u => new UsuarioDTO
            {
                UserId = u.UserId,
                Fullname = u.Fullname,
                Username = u.Username,
                Correo = u.Correo,
                Telefono = u.Telefono,
                Estado = u.Estado,
                BloqueadoHasta = u.BloqueadoHasta,
                Roles = u.RolPermisoUsuarios
                    .GroupBy(rpu => new { rpu.RolPermiso.Rol.ID_Rol, rpu.RolPermiso.Rol.Nombre_Rol })
                    .Select(group => new RolDTO
                    {
                        ID_Rol = group.Key.ID_Rol,
                        Nombre_Rol = group.Key.Nombre_Rol,
                        Permisos = group.Select(rpu => new PermisoDTO
                        {
                            ID_Permiso = rpu.RolPermiso.Permiso.ID_Permiso,
                            Nombre_Permiso = rpu.RolPermiso.Permiso.Nombre_Permiso
                        }).ToList()
                    })
                    .ToList()
            });
        }



        public async Task<UsuarioDTO> GetUsuarioById(int id)
        {
            //return await _context.Usuarios.FindAsync(id);
            var usuario = await _context.Usuarios
                .Where(u => u.UserId == id)
                .Include(u => u.RolPermisoUsuarios)
                    .ThenInclude(rpu => rpu.RolPermiso)
                        .ThenInclude(rp => rp.Rol)
                .Include(u => u.RolPermisoUsuarios)
                    .ThenInclude(rpu => rpu.RolPermiso)
                        .ThenInclude(rp => rp.Permiso)
                .FirstOrDefaultAsync();

            if (usuario == null)
                return null;

            // Agrupar los roles y permisos en memoria
            var roles = usuario.RolPermisoUsuarios
                                .GroupBy(rpu => new { rpu.RolPermiso.Rol.ID_Rol, rpu.RolPermiso.Rol.Nombre_Rol })
                                .Select(group => new RolDTO
                                {
                                    ID_Rol = group.Key.ID_Rol,
                                    Nombre_Rol = group.Key.Nombre_Rol,
                                    Permisos = group.Select(rpu => new PermisoDTO
                                    {
                                        ID_Permiso = rpu.RolPermiso.Permiso.ID_Permiso,
                                        Nombre_Permiso = rpu.RolPermiso.Permiso.Nombre_Permiso
                                    }).ToList()
                                })
                                .ToList();

            return new UsuarioDTO
            {
                UserId = usuario.UserId,
                Fullname = usuario.Fullname,
                Username = usuario.Username,
                Correo = usuario.Correo,
                Telefono = usuario.Telefono,
                Estado = usuario.Estado,
                BloqueadoHasta = usuario.BloqueadoHasta,
                Roles = roles
            };


        }

        public async Task<Usuario> CreateUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task UpdateUsuario(Usuario usuario)
        {
            // Trae el usuario actual de la base para no perder campos que el
            // formulario de edición no envía (como la contraseña).
            var usuarioExistente = await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == usuario.UserId);

            if (usuarioExistente == null)
                return;

            // Si no llega password nueva, conserva la que ya existía.
            if (string.IsNullOrWhiteSpace(usuario.Password))
            {
                usuario.Password = usuarioExistente.Password;
            }

            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ResultadoLogin> Validate(string userName, string password)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(x => x.Username == userName);

            if (usuario == null)
                return new ResultadoLogin { Motivo = MotivoLogin.UsuarioNoExiste };

            if (usuario.BloqueadoHasta.HasValue && usuario.BloqueadoHasta.Value > DateTime.Now)
                return new ResultadoLogin
                {
                    Motivo = MotivoLogin.CuentaBloqueada,
                    BloqueadoHasta = usuario.BloqueadoHasta
                };

            if (usuario.Estado != "activo")
                return new ResultadoLogin { Motivo = MotivoLogin.CuentaInactiva };

            if (usuario.Password != password)
            {
                usuario.IntentosFallidos++;

                if (usuario.IntentosFallidos >= 5)
                {
                    usuario.BloqueadoHasta = DateTime.Now.AddMinutes(5);
                    usuario.IntentosFallidos = 0;
                }

                await _context.SaveChangesAsync();

                return new ResultadoLogin { Motivo = MotivoLogin.PasswordIncorrecta };
            }

            usuario.IntentosFallidos = 0;
            usuario.BloqueadoHasta = null;
            await _context.SaveChangesAsync();

            return new ResultadoLogin { Motivo = MotivoLogin.Exitoso, Usuario = usuario };
        }
        public async Task<bool> DesbloquearUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return false;

            usuario.IntentosFallidos = 0;
            usuario.BloqueadoHasta = null;
            await _context.SaveChangesAsync();

            return true;
        }

        //public bool Validate(string userName, string password)
        //{
        //    var list = _context.Usuarios.ToList();
        //    var access = list.Where(x => x.Username == userName && x.Password == password)
        //        .FirstOrDefault();

        //    if (access != null)
        //        return true;
        //    return false;
        //}



    }
}