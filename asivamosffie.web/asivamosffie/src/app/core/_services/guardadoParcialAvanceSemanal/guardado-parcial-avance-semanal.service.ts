import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { RegistrarAvanceSemanalService } from '../registrarAvanceSemanal/registrar-avance-semanal.service';

@Injectable({
  providedIn: 'root'
})
export class GuardadoParcialAvanceSemanalService {

    private dataAvanceFisico: any;
    private dataAvanceFinanciero: any;
    private dataGestionAmbiental: any;
    private dataGestionCalidad: any;
    private dataGestionSst: any;
    private dataGestionSocial: any;
    private dataAlertasRelevantes: any;
    private dataReporteActividades: any;
    private dataRegistroFotografico: any;
    private dataComiteObra: any;
    private seRealizoPeticion = true;
    private seguimientoSemanalGestionObraId = 0;

    constructor(
        private avanceSemanalSvc: RegistrarAvanceSemanalService,
        private dialog: MatDialog )
    { }

    getDataAvanceFisico( avanceFisico: any, seRealizoPeticion?: boolean ) {
        // console.log( 1, seRealizoPeticion )
        this.dataAvanceFisico = avanceFisico;
        
        if ( seRealizoPeticion !== undefined ) {
            this.seRealizoPeticion = seRealizoPeticion;
        }
    }

    getDataAvanceFinanciero( avanceFinanciero: any, seRealizoPeticion?: boolean ) {
        this.dataAvanceFinanciero = avanceFinanciero;

        if ( seRealizoPeticion !== undefined ) {
            this.seRealizoPeticion = seRealizoPeticion;
        }
    }

    getDataGestionAmbiental( gestionAmbiental: any, seRealizoPeticion?: boolean ) {
      this.dataGestionAmbiental = gestionAmbiental;
      
      if ( seRealizoPeticion !== undefined ) {
        this.seRealizoPeticion = seRealizoPeticion;
      }
    }

    getDataGestionCalidad( gestionCalidad: any, seRealizoPeticion?: boolean ) {
        this.dataGestionCalidad = gestionCalidad;

        if ( seRealizoPeticion !== undefined ) {
            this.seRealizoPeticion = seRealizoPeticion;
        }
    }

    getDataGestionSst( gestionSst: any, seRealizoPeticion?: boolean ) {
        this.dataGestionSst = gestionSst;

        if ( seRealizoPeticion !== undefined ) {
            this.seRealizoPeticion = seRealizoPeticion;
        }
    }

    getDataGestionSocial( gestionSocial: any, seRealizoPeticion?: boolean ) {
        this.dataGestionSocial = gestionSocial;

        if ( seRealizoPeticion !== undefined ) {
            this.seRealizoPeticion = seRealizoPeticion;
        }
    }

    getDataAlertasRelevantes( alertasRelevantes: any, seRealizoPeticion?: boolean ) {
        this.dataAlertasRelevantes = alertasRelevantes;

        if ( seRealizoPeticion !== undefined ) {
            this.seRealizoPeticion = seRealizoPeticion;
        }
    }

    getDataReporteActividades( reporteActividades: any, seRealizoPeticion?: boolean ) {
        this.dataReporteActividades = reporteActividades;

        if ( seRealizoPeticion !== undefined ) {
            this.seRealizoPeticion = seRealizoPeticion;
        }
    }

    getDataRegistroFotografico( registroFotoGrafico: any, seRealizoPeticion?: boolean ) {
        this.dataRegistroFotografico = registroFotoGrafico;

        if ( seRealizoPeticion !== undefined ) {
            this.seRealizoPeticion = seRealizoPeticion;
        }
    }

    getDataComiteObra( comiteObra: any, seRealizoPeticion?: boolean ) {
        this.dataComiteObra = comiteObra;

        if ( seRealizoPeticion !== undefined ) {
            this.seRealizoPeticion = seRealizoPeticion;
        }
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open( ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    openDialogConfirmar(modalTitle: string, modalText: string) {
        return this.dialog.open( ModalDialogComponent, {
          width: '30em',
          data: { modalTitle, modalText, siNoBoton: true }
        } );
    }

    getGuardadoParcial( pSeguimientoSemanal: any ) {
        if ( this.seRealizoPeticion === false ) {
            // Get data tabla avance fisico
            if ( this.dataAvanceFisico !== undefined ) {
                pSeguimientoSemanal.seguimientoSemanalAvanceFisico = this.dataAvanceFisico
            }
            // Get data avance financiero
            if ( this.dataAvanceFinanciero !== undefined ) {
                pSeguimientoSemanal.seguimientoSemanalAvanceFinanciero = this.dataAvanceFinanciero
            }
            // Get data gestion ambiental
            if ( this.dataGestionAmbiental !== undefined ) {
                if ( pSeguimientoSemanal.seguimientoSemanalGestionObra !== undefined ) {
                    if ( pSeguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ) {
                        pSeguimientoSemanal.seguimientoSemanalGestionObra[ 0 ].seguimientoSemanalGestionObraAmbiental = this.dataGestionAmbiental
                    } else {
                        pSeguimientoSemanal.seguimientoSemanalGestionObra = [
                            {
                                seguimientoSemanalId: pSeguimientoSemanal.seguimientoSemanalId,
                                seguimientoSemanalGestionObraId: this.seguimientoSemanalGestionObraId,
                                seguimientoSemanalGestionObraAmbiental: this.dataGestionAmbiental
                            }
                        ];
                    }
                } else {
                    pSeguimientoSemanal.seguimientoSemanalGestionObra = [
                        {
                            seguimientoSemanalId: pSeguimientoSemanal.seguimientoSemanalId,
                            seguimientoSemanalGestionObraId: this.seguimientoSemanalGestionObraId,
                            seguimientoSemanalGestionObraAmbiental: this.dataGestionAmbiental
                        }
                    ];
                }
            }
            // Get data gestion de calidad
            if ( this.dataGestionCalidad !== undefined ) {
                if ( pSeguimientoSemanal.seguimientoSemanalGestionObra !== undefined ) {
                    if ( pSeguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ) {
                        pSeguimientoSemanal.seguimientoSemanalGestionObra[ 0 ].seguimientoSemanalGestionObraCalidad = this.dataGestionCalidad
                    } else {
                        pSeguimientoSemanal.seguimientoSemanalGestionObra = [
                            {
                                seguimientoSemanalId: pSeguimientoSemanal.seguimientoSemanalId,
                                seguimientoSemanalGestionObraId: this.seguimientoSemanalGestionObraId,
                                seguimientoSemanalGestionObraCalidad: this.dataGestionCalidad
                            }
                        ];
                    }
                } else {
                    pSeguimientoSemanal.seguimientoSemanalGestionObra = [
                        {
                            seguimientoSemanalId: pSeguimientoSemanal.seguimientoSemanalId,
                            seguimientoSemanalGestionObraId: this.seguimientoSemanalGestionObraId,
                            seguimientoSemanalGestionObraCalidad: this.dataGestionCalidad
                        }
                    ];
                }
            }
            // Get data gestion de seguridad y salud en el trabajo
            if ( this.dataGestionSst !== undefined ) {
                if ( pSeguimientoSemanal.seguimientoSemanalGestionObra !== undefined ) {
                    if ( pSeguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ) {
                        pSeguimientoSemanal.seguimientoSemanalGestionObra[ 0 ].seguimientoSemanalGestionObraSeguridadSalud = this.dataGestionSst
                    } else {
                        pSeguimientoSemanal.seguimientoSemanalGestionObra = [
                            {
                                seguimientoSemanalId: pSeguimientoSemanal.seguimientoSemanalId,
                                seguimientoSemanalGestionObraId: this.seguimientoSemanalGestionObraId,
                                seguimientoSemanalGestionObraSeguridadSalud: this.dataGestionSst
                            }
                        ];
                    }
                } else {
                    pSeguimientoSemanal.seguimientoSemanalGestionObra = [
                        {
                            seguimientoSemanalId: pSeguimientoSemanal.seguimientoSemanalId,
                            seguimientoSemanalGestionObraId: this.seguimientoSemanalGestionObraId,
                            seguimientoSemanalGestionObraSeguridadSalud: this.dataGestionSst
                        }
                    ];
                }
            }
            // Get data gestion social
            if ( this.dataGestionSocial !== undefined ) {
                if ( pSeguimientoSemanal.seguimientoSemanalGestionObra !== undefined ) {
                    if ( pSeguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ) {
                        pSeguimientoSemanal.seguimientoSemanalGestionObra[ 0 ].seguimientoSemanalGestionObraSocial = this.dataGestionSocial
                    } else {
                        pSeguimientoSemanal.seguimientoSemanalGestionObra = [
                            {
                                seguimientoSemanalId: pSeguimientoSemanal.seguimientoSemanalId,
                                seguimientoSemanalGestionObraId: this.seguimientoSemanalGestionObraId,
                                seguimientoSemanalGestionObraSocial: this.dataGestionSocial
                            }
                        ];
                    }
                } else {
                    pSeguimientoSemanal.seguimientoSemanalGestionObra = [
                        {
                            seguimientoSemanalId: pSeguimientoSemanal.seguimientoSemanalId,
                            seguimientoSemanalGestionObraId: this.seguimientoSemanalGestionObraId,
                            seguimientoSemanalGestionObraSocial: this.dataGestionSocial
                        }
                    ];
                }
            }
            // Get data alerta relevantes
            if ( this.dataAlertasRelevantes !== undefined ) {
                if ( pSeguimientoSemanal.seguimientoSemanalGestionObra !== undefined ) {
                    if ( pSeguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ) {
                        pSeguimientoSemanal.seguimientoSemanalGestionObra[ 0 ].seguimientoSemanalGestionObraAlerta = this.dataAlertasRelevantes
                    } else {
                        pSeguimientoSemanal.seguimientoSemanalGestionObra = [
                            {
                                seguimientoSemanalId: pSeguimientoSemanal.seguimientoSemanalId,
                                seguimientoSemanalGestionObraId: this.seguimientoSemanalGestionObraId,
                                seguimientoSemanalGestionObraAlerta: this.dataAlertasRelevantes
                            }
                        ];
                    }
                } else {
                    pSeguimientoSemanal.seguimientoSemanalGestionObra = [
                        {
                            seguimientoSemanalId: pSeguimientoSemanal.seguimientoSemanalId,
                            seguimientoSemanalGestionObraId: this.seguimientoSemanalGestionObraId,
                            seguimientoSemanalGestionObraAlerta: this.dataAlertasRelevantes
                        }
                    ];
                }
            }
            // Get data reporte de acticidades
            if ( this.dataReporteActividades !== undefined ) {
                pSeguimientoSemanal.seguimientoSemanalReporteActividad = this.dataReporteActividades
            }
            // Get data registro fotografico
            if ( this.dataRegistroFotografico !== undefined ) {
                pSeguimientoSemanal.seguimientoSemanalRegistroFotografico = this.dataRegistroFotografico
            }
            // Get data comite de obra
            if ( this.dataComiteObra !== undefined ) {
                pSeguimientoSemanal.seguimientoSemanalRegistrarComiteObra = this.dataComiteObra
            }

            this.openDialogConfirmar( '', '<b>¿Desea guardar la información registrada?</b>' )
                .afterClosed()
                .subscribe(
                    response => {
                        if ( response === true ) {
                            this.avanceSemanalSvc.saveUpdateSeguimientoSemanal( pSeguimientoSemanal )
                                .subscribe(
                                    response => {
                                        this.seRealizoPeticion = false;
                                        this.openDialog( '', `<b>${ response.message }</b>` )
                                    },
                                    err => this.openDialog( '', `<b>${ err.message }</b>` )
                                );
                        }
                    }
                )
        }
    }

}
