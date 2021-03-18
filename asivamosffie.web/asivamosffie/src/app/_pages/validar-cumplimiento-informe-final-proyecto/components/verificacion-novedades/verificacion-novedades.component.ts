import { Component, Input, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { InformeFinalObservaciones, Report } from 'src/app/_interfaces/proyecto-final.model';

@Component({
  selector: 'app-verificacion-novedades',
  templateUrl: './verificacion-novedades.component.html',
  styleUrls: ['./verificacion-novedades.component.scss']
})
export class VerificacionNovedadesComponent implements OnInit {

  @Input() report: Report
  @Input() existeObservacionCumplimiento: boolean
  ELEMENT_DATA : InformeFinalObservaciones[] = [];
  anexos: any;
  dataSource = new MatTableDataSource<InformeFinalObservaciones>(this.ELEMENT_DATA);

  constructor() { }

  ngOnInit(): void {
    if(this.report != null){
      if(this.report.proyecto.informeFinal[0].observacionVigenteInformeFinalNovedades){
        this.anexos = this.report.proyecto.informeFinal[0].observacionVigenteInformeFinalNovedades;
      }else if(this.report.proyecto.informeFinal[0].historialObsInformeFinalNovedades.length > 0){
        this.anexos = this.report.proyecto.informeFinal[0].historialObsInformeFinalNovedades[0];
      }
    }
  }

}
