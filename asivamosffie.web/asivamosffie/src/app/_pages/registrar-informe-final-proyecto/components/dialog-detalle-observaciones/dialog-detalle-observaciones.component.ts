import { Component, OnInit, Inject } from '@angular/core';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { RegistrarInformeFinalProyectoService } from 'src/app/core/_services/registrar-informe-final-proyecto.service';
@Component({
  selector: 'app-dialog-detalle-observaciones',
  templateUrl: './dialog-detalle-observaciones.component.html',
  styleUrls: ['./dialog-detalle-observaciones.component.scss']
})
export class DialogDetalleObservacionesComponent implements OnInit {
  observaciones: any;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data,
    private registrarInformeFinalProyectoService: RegistrarInformeFinalProyectoService
  ) { }

  ngOnInit(): void {
    this.getInformeFinalAnexoByInformeFinalInterventoriaId(this.data.informeFinalInterventoriaId);
  }

  getInformeFinalAnexoByInformeFinalInterventoriaId (id:string) {
    this.registrarInformeFinalProyectoService.getInformeFinalAnexoByInformeFinalInterventoriaId(id)
    .subscribe(observaciones => {
      this.observaciones = observaciones;
    });
  }
}
