import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-registro-nuevo-proceso-judicial',
  templateUrl: './registro-nuevo-proceso-judicial.component.html',
  styleUrls: ['./registro-nuevo-proceso-judicial.component.scss']
})
export class RegistroNuevoProcesoJudicialComponent implements OnInit {
  addressForm = this.fb.group({
    legitimacion: [null, Validators.required],
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
