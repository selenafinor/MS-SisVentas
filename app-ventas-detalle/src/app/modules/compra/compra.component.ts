import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-compra',
  imports: [RouterModule],
  templateUrl: './compra.component.html',
  styleUrl: './compra.component.css',
  changeDetection: ChangeDetectionStrategy.Default,
})
export class CompraComponent { }