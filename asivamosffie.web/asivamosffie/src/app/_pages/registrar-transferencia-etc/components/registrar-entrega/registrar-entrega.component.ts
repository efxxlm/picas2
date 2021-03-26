import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Params } from '@angular/router';
import { RegisterProjectEtcService } from 'src/app/core/_services/registerProjectETC/register-project-etc.service';
import { ProyectoEntregaETC } from 'src/app/_interfaces/proyecto-entrega-etc';

@Component({
  selector: 'app-registrar-entrega',
  templateUrl: './registrar-entrega.component.html',
  styleUrls: ['./registrar-entrega.component.scss']
})
export class RegistrarEntregaComponent implements OnInit {

  id: number;
  ELEMENT_DATA : ProyectoEntregaETC[] = [];
  numeroContratoObra: string;
  numeroContratoInterventoria: string; 
  llaveMen: string;
  proyectoEntregaEtc: ProyectoEntregaETC;
  semaforoActaBienesServicios = 'sin-diligenciar';
  semaforoRemision = 'sin-diligenciar';
  semaforoRecorrido = 'sin-diligenciar';

  constructor(
    private route: ActivatedRoute,
    public dialog: MatDialog,
    private registerProjectETCService: RegisterProjectEtcService,
  ) { 
    this.route.params.subscribe((params: Params) => {
      this.id = params.id;
      this.getProyectoEntregaETCByInformeFinalId(this.id);
    });
  }
  dataSource = new MatTableDataSource<ProyectoEntregaETC>(this.ELEMENT_DATA);

  ngOnInit(): void {
    console.log("entra");
    this.getProyectoEntregaETCByInformeFinalId(this.id);
  }

  getProyectoEntregaETCByInformeFinalId(id: number){
    this.registerProjectETCService.getProyectoEntregaETCByInformeFinalId(id)
    .subscribe(report => {
      this.llaveMen = report[0].llaveMen;
      this.numeroContratoObra = report[0].numeroContratoObra;
      this.numeroContratoInterventoria = report[0].numeroContratoInterventoria;
      if ( report[0].proyectoEntregaEtc != null ) {

        this.proyectoEntregaEtc = report[0].proyectoEntregaEtc as ProyectoEntregaETC;

        // Semaforo Acta Bienes Servicios
        if ( this.proyectoEntregaEtc.registroCompletoActaBienesServicios === true ) {
          this.semaforoActaBienesServicios = 'completo';
        }
        else if (this.proyectoEntregaEtc.registroCompletoActaBienesServicios === false && 
                (this.proyectoEntregaEtc.actaBienesServicios === null || this.proyectoEntregaEtc.actaBienesServicios === "" || !this.proyectoEntregaEtc.actaBienesServicios
                || this.proyectoEntregaEtc.fechaFirmaActaBienesServicios === null || !this.proyectoEntregaEtc.fechaFirmaActaBienesServicios )) {
            this.semaforoActaBienesServicios = 'en-proceso';
        }
        
        // Semaforo Remision
        if ( this.proyectoEntregaEtc.registroCompletoRemision === true ) {
          this.semaforoRemision = 'completo';
        }else if(this.proyectoEntregaEtc.registroCompletoRemision === false && (this.proyectoEntregaEtc.numRadicadoDocumentosEntregaEtc === null || this.proyectoEntregaEtc.numRadicadoDocumentosEntregaEtc === "" || !this.proyectoEntregaEtc.numRadicadoDocumentosEntregaEtc)
        || this.proyectoEntregaEtc.fechaEntregaDocumentosEtc === null || !this.proyectoEntregaEtc.fechaEntregaDocumentosEtc  ){
          this.semaforoRemision = 'en-proceso';
        }

        // Semaforo recorrido obra
        if ( this.proyectoEntregaEtc.registroCompletoRecorridoObra === true ) {
          this.semaforoRecorrido = 'completo';
        }else if(this.proyectoEntregaEtc.registroCompletoRecorridoObra === false 
                && (this.proyectoEntregaEtc.fechaRecorridoObra === null || this.proyectoEntregaEtc.fechaFirmaActaEngregaFisica === null
                || this.proyectoEntregaEtc.urlActaEntregaFisica === null || this.proyectoEntregaEtc.urlActaEntregaFisica === "" 
                || this.proyectoEntregaEtc.numRepresentantesRecorrido == null)){
          this.semaforoRecorrido = 'en-proceso';
        }else if(this.proyectoEntregaEtc.registroCompletoRecorridoObra === false
          && (this.proyectoEntregaEtc.fechaRecorridoObra === null && this.proyectoEntregaEtc.fechaFirmaActaEngregaFisica === null
          && (this.proyectoEntregaEtc.urlActaEntregaFisica === null || this.proyectoEntregaEtc.urlActaEntregaFisica === "" )
          && this.proyectoEntregaEtc.numRepresentantesRecorrido === null)){
          this.semaforoRecorrido = 'sin-diligenciar';
        }else if (this.proyectoEntregaEtc.registroCompletoRecorridoObra == null){
          this.semaforoRecorrido = 'sin-diligenciar';
        }else{
          this.semaforoRecorrido = 'en-proceso';
        }
      }
    });
  }

}
