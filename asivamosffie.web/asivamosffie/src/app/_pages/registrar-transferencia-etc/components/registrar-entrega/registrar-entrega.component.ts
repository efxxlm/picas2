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
        if (this.proyectoEntregaEtc.actaBienesServicios !== null && this.proyectoEntregaEtc.actaBienesServicios !== ""
             && this.proyectoEntregaEtc.fechaFirmaActaBienesServicios !== null  
             && this.proyectoEntregaEtc.registroCompletoActaBienesServicios == false) {
          this.semaforoActaBienesServicios = 'en-proceso';
        }
        if ( this.proyectoEntregaEtc.registroCompletoActaBienesServicios === true ) {
          this.semaforoActaBienesServicios = 'completo';
        }
        
        // Semaforo Remision
        if (this.proyectoEntregaEtc.numRadicadoDocumentosEntregaEtc !== null && this.proyectoEntregaEtc.numRadicadoDocumentosEntregaEtc !== ""
        && this.proyectoEntregaEtc.fechaEntregaDocumentosEtc !== null  
        && this.proyectoEntregaEtc.registroCompletoRemision == false) {
          this.semaforoRemision = 'en-proceso';
        }
        if ( this.proyectoEntregaEtc.registroCompletoRemision === true ) {
          this.semaforoRemision = 'completo';
        }
      }

    });
  }

}
