import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-titulo',
  templateUrl: './titulo.component.html',
  styleUrls: ['./titulo.component.scss']
})
export class TituloComponent implements OnInit {

  tipoDisponibilidad: FormControl;

  selectDisponibilidad = [
    { name: 'DDP tradicional', value: 1 },
    { name: 'DDP especial', value: 2 }
  ];

  constructor(private router: Router) { }

  ngOnInit(): void {
    this.declararSelect();
  }

  private declararSelect() {
    this.tipoDisponibilidad = new FormControl('', [Validators.required]);
  }

  crearSolicitud() {
    switch (this.tipoDisponibilidad.value) {
      case 1:
        this.router.navigate(['/solicitar-disponibilidad-presupuestal/crear-solicitud-tradicional']);
        break;
      case 2:
        this.router.navigate(['/solicitar-disponibilidad-presupuestal/crear-solicitud-especial']);
        break;
    }
  }

}
