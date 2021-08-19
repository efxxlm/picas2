import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import html2canvas from 'html2canvas';
import jsPDF from 'jspdf';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { RegistrarAvanceSemanalService } from 'src/app/core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';

@Component({
  selector: 'app-reporte-semanal',
  templateUrl: './reporte-semanal.component.html',
  styleUrls: ['./reporte-semanal.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class ReporteSemanalComponent implements OnInit {

  contratacionProyectoId: string;
  seguimientoSemanalId: string;
  dataReporteSemanal: any;
  seguimientoSemanalGestionObraId: number;
  cantidadActividades = 0;
  gestionObraAmbiental: any;
  gestionAmbiental: boolean;
  gestionSST: any;
  gestionSocial: any;
  gestionAmbientalId = 0; // ID gestion ambiental.
  manejoMaterialInsumoId = 0; // ID manejo de materiales  e insumos.
  residuosConstruccionId = 0; // ID residuos de construccion.
  residuosPeligrososId = 0; // ID residuos peligrosos.
  manejoOtrosId = 0; // ID manejo de otros.

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private commonSvc: CommonService,
    private registrarAvanceSemanalService: RegistrarAvanceSemanalService
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.contratacionProyectoId = params.pContratacionProyectoId;
      this.seguimientoSemanalId = params.pSeguimientoSemanalId;
      console.log(this.contratacionProyectoId);
      console.log(this.seguimientoSemanalId);
      //this.getLastSeguimientoSemanalContratacionProyectoIdOrSeguimientoSemanalId(this.contratacionProyectoId, this.seguimientoSemanalId)

    });
  }

  getLastSeguimientoSemanalContratacionProyectoIdOrSeguimientoSemanalId( pContratacionProyectoId, pSeguimientoSemanalId ) {
    //0-156
    console.log(pContratacionProyectoId);
    this.registrarAvanceSemanalService.getLastSeguimientoSemanalContratacionProyectoIdOrSeguimientoSemanalId(pContratacionProyectoId, pSeguimientoSemanalId).subscribe(
      response => {
        this.dataReporteSemanal = response;
        if ( this.dataReporteSemanal !== undefined ) {
          this.seguimientoSemanalId = this.dataReporteSemanal.seguimientoSemanalId;
          this.seguimientoSemanalGestionObraId =  this.dataReporteSemanal.seguimientoSemanalGestionObra.length > 0 ?
          this.dataReporteSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraId : 0;
          if ( this.dataReporteSemanal.seguimientoSemanalGestionObra.length > 0 && this.dataReporteSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraAmbiental.length > 0 ) {
              this.cantidadActividades = 0;
              this.gestionObraAmbiental = this.dataReporteSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraAmbiental[0];

              if ( this.gestionObraAmbiental.seEjecutoGestionAmbiental !== undefined ) {
                  this.gestionAmbiental = true;
              } else {
                  this.gestionAmbiental = false;
              }

              if ( this.gestionObraAmbiental.seEjecutoGestionAmbiental === false ) {
                  // ID gestionAmbiental
                  this.gestionAmbientalId = this.gestionObraAmbiental.seguimientoSemanalGestionObraAmbientalId;
                  /* Verificacion con Julian Martinez y John Portela
                  if ( this.esVerDetalle === false ) {
                      this.registrarAvanceSemanalService.getObservacionSeguimientoSemanal(parseInt(this.seguimientoSemanalId), this.gestionAmbientalId, this.tipoObservacionAmbiental.gestionAmbientalCodigo )
                          .subscribe(
                              response => {
                                  this.obsApoyo = response.find( obs => obs.archivada === false && obs.esSupervisor === false );
                                  this.obsSupervisor = response.find( obs => obs.archivada === false && obs.esSupervisor === true );
                                  this.historialGestionAmbiental = response;

                                  if ( this.obsApoyo !== undefined || this.obsSupervisor !== undefined ) {
                                      this.tieneObservacion.emit();
                                  }

                                  this.tablaHistorialgestionAmbiental = new MatTableDataSource( this.historialGestionAmbiental );
                              }
                          );
                  }
                  */
              }
              if ( this.gestionObraAmbiental.tieneManejoMaterialesInsumo === true ) {
                  // ID manejo de materiales e insumos
                  if ( this.gestionObraAmbiental.manejoMaterialesInsumo !== undefined ) {
                      this.manejoMaterialInsumoId = this.gestionObraAmbiental.manejoMaterialesInsumo.manejoMaterialesInsumosId;
                  }

                  this.cantidadActividades++;
              }
              if ( this.gestionObraAmbiental.tieneManejoResiduosConstruccionDemolicion === true ) {
                  // ID residuos construccion
                  if ( this.gestionObraAmbiental.manejoResiduosConstruccionDemolicion !== undefined ) {
                      this.residuosConstruccionId = this.gestionObraAmbiental.manejoResiduosConstruccionDemolicion.manejoResiduosConstruccionDemolicionId;
                  }

                  this.cantidadActividades++;
              }
              if ( this.gestionObraAmbiental.tieneManejoResiduosPeligrososEspeciales === true ) {
                  // ID residuos peligrosos
                  if ( this.gestionObraAmbiental.manejoResiduosPeligrososEspeciales !== undefined ) {
                      this.residuosPeligrososId = this.gestionObraAmbiental.manejoResiduosPeligrososEspeciales.manejoResiduosPeligrososEspecialesId;
                  }

                  this.cantidadActividades++;
              }
              if ( this.gestionObraAmbiental.tieneManejoOtro === true ) {
                  // ID manejo de otros
                  if ( this.gestionObraAmbiental.manejoOtro !== undefined ) {
                      this.manejoOtrosId = this.gestionObraAmbiental.manejoOtro.manejoOtroId;
                  }

                  this.cantidadActividades++;
              }
              if ( this.gestionObraAmbiental.seEjecutoGestionAmbiental === true ) {
                  if ( this.cantidadActividades > 0 ) {
                    this.cantidadActividades = this.cantidadActividades;
                  }
              }
          }
          this.gestionSST = this.dataReporteSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraSeguridadSalud[0];
          this.gestionSocial = this.dataReporteSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraSocial[0];
      }
    })
  }

  downloadPDF() {
    /*
   html2canvas( document.getElementById( 'example-pdf' ) ).then(canvas => {
       const contentDataURL = canvas.toDataURL('image/jpeg', 1.0)

      let pdf = new jsPDF('l', 'cm', 'a4', false); //Generates PDF in landscape mode
    //   // let pdf = new jsPDF('p', 'cm', 'a4'); //Generates PDF in portrait mode
      pdf.addImage(contentDataURL, 'PNG', 0, 0, 29.8, 21.5, 'undefined', 'FAST')
      pdf.save('Filename.pdf');
    });
    */
    console.log(document.getElementById('example-pdf').innerHTML);
    const pdfHTML = document.getElementById('example-pdf').innerHTML;
    const pdf = {
      EsHorizontal: true,
      MargenArriba: 2,
      MargenAbajo: 2,
      MargenDerecha: 2,
      MargenIzquierda: 2,
      Contenido: pdfHTML
    };

    this.commonSvc.GetHtmlToPdf(pdf).subscribe(
      response => {
        const documento = `OrdernGiro.pdf`;
        const text = documento,
          blob = new Blob([response], { type: 'application/pdf' }),
          anchor = document.createElement('a');
        anchor.download = documento;
        anchor.href = window.URL.createObjectURL(blob);
        anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
        anchor.click();
      },
      e => {
        console.log(e);
      }
    );
    
  }

}
