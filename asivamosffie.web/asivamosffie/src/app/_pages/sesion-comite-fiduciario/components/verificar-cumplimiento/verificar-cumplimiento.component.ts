import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormArray } from '@angular/forms';

@Component({
  selector: 'app-verificar-cumplimiento',
  templateUrl: './verificar-cumplimiento.component.html',
  styleUrls: ['./verificar-cumplimiento.component.scss']
})
export class VerificarCumplimientoComponent implements OnInit {

  data: any[] = [
    {
      tarea: 'Realizar seguimiento semanal del cronograma',
      responsable: 'María josé Coronado',
      fechaCumplimiento: '17/07/2020 ',
      fechaReporte: '02/07/2020',
      estadoReporte: 'Finalizada'
    }
  ]

  constructor() { }

  ngOnInit(): void {
  }

};