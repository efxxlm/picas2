import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-ver-detalle-editar-registro-proceso-dj',
  templateUrl: './ver-detalle-editar-registro-proceso-dj.component.html',
  styleUrls: ['./ver-detalle-editar-registro-proceso-dj.component.scss']
})
export class VerDetalleEditarRegistroProcesoDjComponent implements OnInit {

  addressForm = this.fb.group({
    legitimacionActiva: [null, Validators.required],
    tipoProceso: [null, Validators.required],
  });
  tipoProcesoArray = [
    { name: 'Laboral', value: '1' },
    { name: 'Civil', value: '2' }
  ];
  constructor(private fb: FormBuilder, public dialog: MatDialog) { }

  ngOnInit(): void {
  }

}
