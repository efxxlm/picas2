import { Component, Input, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ValidarInformeFinalService } from 'src/app/core/_services/validarInformeFinal/validar-informe-final.service';
import { InformeFinal } from 'src/app/_interfaces/informe-final';

@Component({
  selector: 'app-tabla-observaciones-cumplimiento-interventoria',
  templateUrl: './tabla-observaciones-cumplimiento-interventoria.component.html',
  styleUrls: ['./tabla-observaciones-cumplimiento-interventoria.component.scss']
})
export class TablaObservacionesCumplimientoInterventoriaComponent implements OnInit {

  @Input() id: number
  ELEMENT_DATA : InformeFinal[] = [];
  anexos: any;
  dataSource = new MatTableDataSource<InformeFinal>(this.ELEMENT_DATA);
  existe_historial = false;

  constructor(
    private validarInformeFinalProyectoService: ValidarInformeFinalService,

  ) { }

  ngOnInit(): void {
    this.getListInformeFinalObservacionesInterventoria(this.id);
  }

  getListInformeFinalObservacionesInterventoria (id:number) {
    this.validarInformeFinalProyectoService.getListInformeFinalObservacionesInterventoria(id)
    .subscribe(informeFinal => {
      if(informeFinal != null){
        this.dataSource.data = informeFinal as InformeFinal[];
        this.anexos = informeFinal;

        if(this.anexos.historialInformeFinalObservacionesInterventoria != null){
          if(this.anexos.historialInformeFinalObservacionesInterventoria.length > 0){
            if(this.anexos.observacionVigenteInformeFinalObservacionesInterventoria != null){
              this.existe_historial = true;
            }
          }      
        }
      }
    });
  }

}
