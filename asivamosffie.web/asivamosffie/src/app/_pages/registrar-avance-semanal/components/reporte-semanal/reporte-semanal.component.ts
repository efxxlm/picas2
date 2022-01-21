import { Component, OnInit, AfterViewInit, OnDestroy, ViewEncapsulation } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import html2canvas from 'html2canvas';
import jsPDF from 'jspdf';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { RegistrarAvanceSemanalService } from 'src/app/core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { environment } from 'src/environments/environment';


@Component({
  selector: 'app-reporte-semanal',
  templateUrl: './reporte-semanal.component.html',
  styleUrls: ['./reporte-semanal.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ReporteSemanalComponent implements OnInit, AfterViewInit, OnDestroy {
  contratacionProyectoId: string;
  seguimientoSemanalId: string;
  dataReporteSemanal: any;
  infoGeneralObra: any;
  infoGeneralInterventoria: any;
  avanceFisico: any[] = [];
  seguimientoSemanalGestionObraId: number;
  cantidadActividades = 0;
  gestionObraAmbiental: any;
  gestionAmbiental: boolean;
  gestionCalidad: any;
  gestionSST: any;
  gestionSocial: any;
  actividadesARealizar: any;
  registroFotografico: any;
  gestionAmbientalId = 0; // ID gestion ambiental.
  manejoMaterialInsumoId = 0; // ID manejo de materiales  e insumos.
  residuosConstruccionId = 0; // ID residuos de construccion.
  residuosPeligrososId = 0; // ID residuos peligrosos.
  manejoOtrosId = 0; // ID manejo de otros.
  avanceFisicoGrafica = '';
  seguimientoFinancieroGrafica = '';
  public sumaTotal;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private commonSvc: CommonService,
    private registrarAvanceSemanalService: RegistrarAvanceSemanalService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.contratacionProyectoId = params.pContratacionProyectoId;
      this.seguimientoSemanalId = params.pSeguimientoSemanalId;
      // console.log(this.contratacionProyectoId);
      // console.log(this.seguimientoSemanalId);
      /*this.getLastSeguimientoSemanalContratacionProyectoIdOrSeguimientoSemanalId(
        this.contratacionProyectoId,
        this.seguimientoSemanalId
      );*/
      this.getSeguimientoSemanalBySeguimientoSemanalId(
        this.seguimientoSemanalId
      );

    });
  }

  ngOnDestroy() {
    location.reload();
  }

  ngAfterViewInit() {
    // setTimeout(this.downloadPDF, 2000);
  }

  getSeguimientoSemanalBySeguimientoSemanalId(
    pSeguimientoSemanalId
  ) {
    //0-156
    // console.log(pContratacionProyectoId);
    this.registrarAvanceSemanalService
      .getSeguimientoSemanalBySeguimientoSemanalId(
        pSeguimientoSemanalId
      )
      .subscribe(response => {
        this.dataReporteSemanal = response;
        console.log('dataReporteSemanal', this.dataReporteSemanal);
        if (this.dataReporteSemanal !== undefined) {
          this.infoGeneralObra = this.dataReporteSemanal.informacionGeneral[0][0];
          this.infoGeneralInterventoria = this.dataReporteSemanal.informacionGeneral[1][0];
          this.seguimientoSemanalId = this.dataReporteSemanal.seguimientoSemanalId;
          this.avanceFisico = this.dataReporteSemanal.avanceFisico;
          this.calcularTotal(this.avanceFisico);
          this.seguimientoSemanalGestionObraId =
            this.dataReporteSemanal.seguimientoSemanalGestionObra.length > 0
              ? this.dataReporteSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraId
              : 0;
          if (
            this.dataReporteSemanal.seguimientoSemanalGestionObra.length > 0 &&
            this.dataReporteSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraAmbiental.length > 0
          ) {
            this.cantidadActividades = 0;
            this.gestionObraAmbiental =
              this.dataReporteSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraAmbiental[0];

            if (this.gestionObraAmbiental.seEjecutoGestionAmbiental !== undefined) {
              this.gestionAmbiental = true;
            } else {
              this.gestionAmbiental = false;
            }

            if (this.gestionObraAmbiental.seEjecutoGestionAmbiental === false) {
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
            if (this.gestionObraAmbiental.tieneManejoMaterialesInsumo === true) {
              // ID manejo de materiales e insumos
              if (this.gestionObraAmbiental.manejoMaterialesInsumo !== undefined) {
                this.manejoMaterialInsumoId =
                  this.gestionObraAmbiental.manejoMaterialesInsumo.manejoMaterialesInsumosId;
              }

              this.cantidadActividades++;
            }
            if (this.gestionObraAmbiental.tieneManejoResiduosConstruccionDemolicion === true) {
              // ID residuos construccion
              if (this.gestionObraAmbiental.manejoResiduosConstruccionDemolicion !== undefined) {
                this.residuosConstruccionId =
                  this.gestionObraAmbiental.manejoResiduosConstruccionDemolicion.manejoResiduosConstruccionDemolicionId;
              }

              this.cantidadActividades++;
            }
            if (this.gestionObraAmbiental.tieneManejoResiduosPeligrososEspeciales === true) {
              // ID residuos peligrosos
              if (this.gestionObraAmbiental.manejoResiduosPeligrososEspeciales !== undefined) {
                this.residuosPeligrososId =
                  this.gestionObraAmbiental.manejoResiduosPeligrososEspeciales.manejoResiduosPeligrososEspecialesId;
              }

              this.cantidadActividades++;
            }
            if (this.gestionObraAmbiental.tieneManejoOtro === true) {
              // ID manejo de otros
              if (this.gestionObraAmbiental.manejoOtro !== undefined) {
                this.manejoOtrosId = this.gestionObraAmbiental.manejoOtro.manejoOtroId;
              }

              this.cantidadActividades++;
            }
            if (this.gestionObraAmbiental.seEjecutoGestionAmbiental === true) {
              if (this.cantidadActividades > 0) {
                this.cantidadActividades = this.cantidadActividades;
              }
            }
          }

          if (this.dataReporteSemanal.seguimientoSemanalGestionObra.length > 0) {
            this.gestionCalidad =
              this.dataReporteSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraCalidad[0];
            this.gestionSST =
              this.dataReporteSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraSeguridadSalud[0];
            this.gestionSocial =
              this.dataReporteSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraSocial[0];
            this.actividadesARealizar = this.dataReporteSemanal.seguimientoSemanalReporteActividad[0];
            this.registroFotografico = this.dataReporteSemanal.seguimientoSemanalRegistroFotografico[0];
          }

          this.avanceFisicoGrafica = this.dataReporteSemanal.avanceFisicoGrafica
            ? this.dataReporteSemanal.avanceFisicoGrafica.split(environment.ruta)[1]
            : '';
          this.seguimientoFinancieroGrafica = this.dataReporteSemanal.seguimientoFinancieroGrafica
            ? this.dataReporteSemanal.seguimientoFinancieroGrafica.split(environment.ruta)[1]
            : '';
        }

        setTimeout(this.downloadPDF, 2000);

      });
  }

  getLastSeguimientoSemanalContratacionProyectoIdOrSeguimientoSemanalId(
    pContratacionProyectoId,
    pSeguimientoSemanalId
  ) {
    //0-156
    // console.log(pContratacionProyectoId);
    this.registrarAvanceSemanalService
      .getLastSeguimientoSemanalContratacionProyectoIdOrSeguimientoSemanalId(
        pContratacionProyectoId,
        pSeguimientoSemanalId
      )
      .subscribe(response => {
        this.dataReporteSemanal = response;
        console.log('dataReporteSemanal', this.dataReporteSemanal);
        if (this.dataReporteSemanal !== undefined) {
          this.infoGeneralObra = this.dataReporteSemanal.informacionGeneral[0][0];
          this.infoGeneralInterventoria = this.dataReporteSemanal.informacionGeneral[1][0];
          this.seguimientoSemanalId = this.dataReporteSemanal.seguimientoSemanalId;
          this.avanceFisico = this.dataReporteSemanal.avanceFisico;
          this.calcularTotal(this.avanceFisico);
          this.seguimientoSemanalGestionObraId =
            this.dataReporteSemanal.seguimientoSemanalGestionObra.length > 0
              ? this.dataReporteSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraId
              : 0;
          if (
            this.dataReporteSemanal.seguimientoSemanalGestionObra.length > 0 &&
            this.dataReporteSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraAmbiental.length > 0
          ) {
            this.cantidadActividades = 0;
            this.gestionObraAmbiental =
              this.dataReporteSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraAmbiental[0];

            if (this.gestionObraAmbiental.seEjecutoGestionAmbiental !== undefined) {
              this.gestionAmbiental = true;
            } else {
              this.gestionAmbiental = false;
            }

            if (this.gestionObraAmbiental.seEjecutoGestionAmbiental === false) {
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
            if (this.gestionObraAmbiental.tieneManejoMaterialesInsumo === true) {
              // ID manejo de materiales e insumos
              if (this.gestionObraAmbiental.manejoMaterialesInsumo !== undefined) {
                this.manejoMaterialInsumoId =
                  this.gestionObraAmbiental.manejoMaterialesInsumo.manejoMaterialesInsumosId;
              }

              this.cantidadActividades++;
            }
            if (this.gestionObraAmbiental.tieneManejoResiduosConstruccionDemolicion === true) {
              // ID residuos construccion
              if (this.gestionObraAmbiental.manejoResiduosConstruccionDemolicion !== undefined) {
                this.residuosConstruccionId =
                  this.gestionObraAmbiental.manejoResiduosConstruccionDemolicion.manejoResiduosConstruccionDemolicionId;
              }

              this.cantidadActividades++;
            }
            if (this.gestionObraAmbiental.tieneManejoResiduosPeligrososEspeciales === true) {
              // ID residuos peligrosos
              if (this.gestionObraAmbiental.manejoResiduosPeligrososEspeciales !== undefined) {
                this.residuosPeligrososId =
                  this.gestionObraAmbiental.manejoResiduosPeligrososEspeciales.manejoResiduosPeligrososEspecialesId;
              }

              this.cantidadActividades++;
            }
            if (this.gestionObraAmbiental.tieneManejoOtro === true) {
              // ID manejo de otros
              if (this.gestionObraAmbiental.manejoOtro !== undefined) {
                this.manejoOtrosId = this.gestionObraAmbiental.manejoOtro.manejoOtroId;
              }

              this.cantidadActividades++;
            }
            if (this.gestionObraAmbiental.seEjecutoGestionAmbiental === true) {
              if (this.cantidadActividades > 0) {
                this.cantidadActividades = this.cantidadActividades;
              }
            }
          }

          if (this.dataReporteSemanal.seguimientoSemanalGestionObra.length > 0) {
            this.gestionCalidad =
              this.dataReporteSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraCalidad[0];
            this.gestionSST =
              this.dataReporteSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraSeguridadSalud[0];
            this.gestionSocial =
              this.dataReporteSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraSocial[0];
            this.actividadesARealizar = this.dataReporteSemanal.seguimientoSemanalReporteActividad[0];
            this.registroFotografico = this.dataReporteSemanal.seguimientoSemanalRegistroFotografico[0];
          }

          this.avanceFisicoGrafica = this.dataReporteSemanal.actividadesARealizar
            ? this.dataReporteSemanal.actividadesARealizar.split(environment.ruta)[1]
            : '';
          this.seguimientoFinancieroGrafica = this.dataReporteSemanal.seguimientoFinancieroGrafica
            ? this.dataReporteSemanal.seguimientoFinancieroGrafica.split(environment.ruta)[1]
            : '';
        }
      });
  }
  calcularTotal(data) {
    let total = 0;
    data.forEach(element => {
      total += element.programacion;
      this.sumaTotal = total;
    });
  }
  downloadPDF() {
    window.print();
    /*
   html2canvas( document.getElementById( 'example-pdf' ) ).then(canvas => {
       const contentDataURL = canvas.toDataURL('image/jpeg', 1.0)

      let pdf = new jsPDF('l', 'cm', 'a4', false); //Generates PDF in landscape mode
    //   // let pdf = new jsPDF('p', 'cm', 'a4'); //Generates PDF in portrait mode
      pdf.addImage(contentDataURL, 'PNG', 0, 0, 29.8, 21.5, 'undefined', 'FAST')
      pdf.save('Filename.pdf');
    });
    */
    // console.log(document.getElementById('example-pdf').innerHTML);
    // const pdfHTML = document.getElementById('example-pdf').innerHTML;
    // const pdf = {
    //   EsHorizontal: true,
    //   MargenArriba: 2,
    //   MargenAbajo: 2,
    //   MargenDerecha: 2,
    //   MargenIzquierda: 2,
    //   Contenido: pdfHTML
    // };
    /*
    public int PlantillaId { get; set; }
    public string Nombre { get; set; }
    public string Codigo { get; set; }
    public string Contenido { get; set; }
    public double? MargenArriba { get; set; }
    public double? MargenAbajo { get; set; }
    public double? MargenDerecha { get; set; }
    public double? MargenIzquierda { get; set; }
    public int? EncabezadoId { get; set; }
    public int? PieDePaginaId { get; set; }
    public int? TipoPlantillaId { get; set; }
    */

    // this.commonSvc.GetHtmlToPdf(pdf).subscribe(
    //   response => {
    //     const documento = `Seguimiento Semanal.pdf`;
    //     const text = documento,
    //       blob = new Blob([response], { type: 'application/pdf' }),
    //       anchor = document.createElement('a');
    //     anchor.download = documento;
    //     anchor.href = window.URL.createObjectURL(blob);
    //     anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
    //     anchor.click();
    //   },
    //   e => {
    //     console.log(e);
    //   }
    // );
  }
}
