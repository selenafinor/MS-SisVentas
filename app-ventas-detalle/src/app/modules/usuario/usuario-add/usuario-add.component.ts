import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { UsuarioService } from '../usuario.service';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-usuario-add',
  imports: [FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './usuario-add.component.html',
  styleUrl: './usuario-add.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UsuarioAddComponent implements OnInit {
  userForm!: FormGroup;
  showPassword = false;

  constructor(
    private fb: FormBuilder,
    private usuarioService: UsuarioService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.userForm = this.fb.group({
      fullname: ['', [Validators.required, Validators.maxLength(100)]],
      username: ['', [Validators.required, Validators.maxLength(50)]],
      correo: ['', [Validators.required, Validators.email]],
      telefono: [''],
      password: ['', [
        Validators.required,
        Validators.minLength(8),
        this.passwordValidator
      ]],
    });
  }

  passwordValidator(control: AbstractControl): ValidationErrors | null {
    const value = control.value || '';
    const hasUpperCase = /[A-Z]/.test(value);
    const hasNumber = /[0-9]/.test(value);
    const hasSymbol = /[!@#$%^&*]/.test(value);
    const minLength = value.length >= 8;

    if (!hasUpperCase || !hasNumber || !hasSymbol || !minLength) {
      return { weakPassword: true };
    }
    return null;
  }

  get passwordStrength(): number {
    const value = this.userForm.get('password')?.value || '';
    let strength = 0;
    if (value.length >= 8) strength++;
    if (/[A-Z]/.test(value)) strength++;
    if (/[0-9]/.test(value)) strength++;
    if (/[!@#$%^&*]/.test(value)) strength++;
    if (value.length >= 12) strength++;
    return strength;
  }

  get strengthLabel(): string {
    const s = this.passwordStrength;
    if (s <= 1) return 'Muy débil';
    if (s === 2) return 'Débil';
    if (s === 3) return 'Regular';
    if (s === 4) return 'Fuerte';
    return 'Muy fuerte';
  }

  get strengthColor(): string {
    const s = this.passwordStrength;
    if (s <= 1) return 'bg-red-500';
    if (s === 2) return 'bg-orange-500';
    if (s === 3) return 'bg-yellow-500';
    if (s === 4) return 'bg-blue-500';
    return 'bg-green-500';
  }
  hasUpperCase(): boolean {
  return /[A-Z]/.test(this.userForm.get('password')?.value || '');
}

hasNumber(): boolean {
  return /[0-9]/.test(this.userForm.get('password')?.value || '');
}

hasSymbol(): boolean {
  return /[!@#$%^&*]/.test(this.userForm.get('password')?.value || '');
}

hasMinLength(): boolean {
  return (this.userForm.get('password')?.value || '').length >= 8;
}

  createUser(): void {
    if (this.userForm.valid) {
      const user = this.userForm.value;
      this.usuarioService.createUsuario(user).subscribe({
        next: () => {
          Swal.fire({
            icon: 'success',
            title: 'Usuario creado',
            text: 'El usuario se ha creado exitosamente.',
            confirmButtonText: 'OK'
          }).then(() => {
            this.router.navigate(['/dashboard/user']);
          });
        },
        error: (err) => {
          console.error(err);
          Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Hubo un problema al crear el usuario.',
            confirmButtonText: 'OK'
          });
        }
      });
    }
  }

  goBack() {
    this.router.navigate(['/dashboard/user']);
  }
}