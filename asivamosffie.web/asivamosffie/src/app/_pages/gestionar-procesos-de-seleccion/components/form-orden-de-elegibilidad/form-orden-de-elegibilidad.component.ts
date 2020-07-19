import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { CargarOrdenDeElegibilidadComponent } from '../cargar-orden-de-elegibilidad/cargar-orden-de-elegibilidad.component';

@Component({
  selector: 'app-form-orden-de-elegibilidad',
  templateUrl: './form-orden-de-elegibilidad.component.html',
  styleUrls: ['./form-orden-de-elegibilidad.component.scss']
})
export class FormOrdenDeElegibilidadComponent {

  selectTipoProceso: FormControl;

  ValueTiposProceso = [
    { name: 'Banco de oferentes', value: 1 }, { name: 'Único contratista seleccionado', value: 2 }
  ];

  constructor(
    public dialog: MatDialog
  ) {
    this.declararSelect();
  }

  private declararSelect() {
    this.selectTipoProceso = new FormControl('', [Validators.required]);
  }

  openCargarElegibilidad() {
    this.dialog.open(CargarOrdenDeElegibilidadComponent);
  }

}
