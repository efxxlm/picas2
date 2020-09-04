import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { DisponibilidadPresupuestalService } from 'src/app/core/_services/disponibilidadPresupuestal/disponibilidad-presupuestal.service';

@Component({
  selector: 'app-validar',
  templateUrl: './validar.component.html',
  styleUrls: ['./validar.component.scss']
})
export class ValidarComponent implements OnInit {

  verAyuda = false;
  listaDisponibilidades: any;

  constructor(private disponibilidadServices: DisponibilidadPresupuestalService) {}

  ngOnInit(): void {
    this.disponibilidadServices.GetListGenerarDisponibilidadPresupuestal().subscribe(respuesta => 
      {
        this.listaDisponibilidades=respuesta;
      }
    );
  }
}
