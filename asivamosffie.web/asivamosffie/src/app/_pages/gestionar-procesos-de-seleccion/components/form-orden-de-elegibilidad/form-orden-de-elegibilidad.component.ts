import { Component, Input } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { EstadosProcesoSeleccion, ProcesoSeleccion } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
import { CargarOrdenDeElegibilidadComponent } from '../cargar-orden-de-elegibilidad/cargar-orden-de-elegibilidad.component';

@Component({
  selector: 'app-form-orden-de-elegibilidad',
  templateUrl: './form-orden-de-elegibilidad.component.html',
  styleUrls: ['./form-orden-de-elegibilidad.component.scss']
})
export class FormOrdenDeElegibilidadComponent {

  selectTipoProceso: FormControl;
  estadosProcesoSeleccion = EstadosProcesoSeleccion;
  @Input() procesoSeleccion: ProcesoSeleccion;
  //@Output() guardar: EventEmitter<any> = new EventEmitter(); 

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
    const dialogRef = this.dialog.open(CargarOrdenDeElegibilidadComponent, {
      width: '70em',
      data: { procesoSeleccionId: this.procesoSeleccion.procesoSeleccionId },
      maxHeight: '90em',
    });
    dialogRef.afterClosed().subscribe(result => {
      location.reload();
    });
  }
  descargaPlantilla()
  {
    location.href ="./assets/files/Formato_Orden_elegibilidad.xlsx";
  }
}
