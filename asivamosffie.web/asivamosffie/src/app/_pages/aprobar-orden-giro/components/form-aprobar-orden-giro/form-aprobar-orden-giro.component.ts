import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { ObservacionesOrdenGiroService } from 'src/app/core/_services/observacionesOrdenGiro/observaciones-orden-giro.service';
import { OrdenPagoService } from 'src/app/core/_services/ordenPago/orden-pago.service';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ListaMenu, ListaMenuId, TipoObservaciones, TipoObservacionesCodigo } from 'src/app/_interfaces/estados-solicitudPago-ordenGiro.interface';

@Component({
  selector: 'app-form-aprobar-orden-giro',
  templateUrl: './form-aprobar-orden-giro.component.html',
  styleUrls: ['./form-aprobar-orden-giro.component.scss']
})
export class FormAprobarOrdenGiroComponent implements OnInit {

    solicitudPago: any;
    contrato: any;
    esRegistroNuevo = false;
    esVerDetalle = false;
    esExpensas = false;
    semaforoInformacionGeneral = 'sin-diligenciar';
    semaforoDetalleGiro = 'sin-diligenciar';
    listaModalidadContrato: Dominio[] = [];
    listaMenu: ListaMenu = ListaMenuId;
    ordenGiroObservacionId = 0;
    ordenGiroId: 0;
    tipoObservaciones: TipoObservaciones = TipoObservacionesCodigo;
    listaDetalleGiro: { contratacionProyectoId: number, llaveMen: string, fases: any[], semaforoDetalle: string }[] = [];
    tablaHistorial = new MatTableDataSource();
    historialObservaciones: any[] = [];
    formObservacion: FormGroup = this.fb.group({
        tieneObservaciones: [ null, Validators.required ],
        observaciones: [ null, Validators.required ],
        fechaCreacion: [ null ]
    });
    displayedColumnsHistorial: string[]  = [
        'fechaRevision',
        'responsable',
        'historial'
    ];
    editorStyle = {
        height: '100px'
    };
    config = {
        toolbar: [
            ['bold', 'italic', 'underline'],
            [{ list: 'ordered' }, { list: 'bullet' }],
            [{ indent: '-1' }, { indent: '+1' }],
            [{ align: [] }],
        ]
    };

    constructor(
        private activatedRoute: ActivatedRoute,
        private ordenGiroSvc: OrdenPagoService,
        private commonSvc: CommonService,
        private registrarPagosSvc: RegistrarRequisitosPagoService,
        private fb: FormBuilder,
        private obsOrdenGiro: ObservacionesOrdenGiroService,
        private dialog: MatDialog,
        private routes: Router )
    {
        // Verificar si es registro nuevo o ver detalle/editar o ver detalle
        this.activatedRoute.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
          if ( urlSegment.path === 'aprobarOrdenGiro' ) {
            this.esRegistroNuevo = true;
          }
          if ( urlSegment.path === 'verDetalle' ) {
            this.esVerDetalle = true;
          }
          if ( urlSegment.path === 'aprobarOrdenGiroExpensas' || urlSegment.path === 'editarOrdenGiroExpensas' ) {
            this.esExpensas = true;
          }
          if ( urlSegment.path === 'verDetalleExpensas' ) {
              this.esExpensas = true;
              this.esVerDetalle = true;
          }
        } );
        // Get lista modalidades de contrato
        this.commonSvc.modalidadesContrato()
            .subscribe( modalidadesContrato => this.listaModalidadContrato = modalidadesContrato );
        // Get solicitud de pago y orden de giro
        this.ordenGiroSvc.getSolicitudPagoBySolicitudPagoId( this.activatedRoute.snapshot.params.id )
            .subscribe(
                async response => {
                    this.solicitudPago = response;
                    this.contrato = response[ 'contratoSon' ];
                    console.log( this.solicitudPago );
                    this.ordenGiroId = this.solicitudPago.ordenGiroId

                    /*
                        Se crea un arreglo de proyectos asociados a una fase y unos criterios que estan asociados a esa fase y al proyecto para
                        el nuevo flujo de Orden de Giro el cual los acordeones de "Estrategia de pagos, Observaciones y Soporte de orden de giro" ya no son hijos del acordeon
                        "Detalle de giro" y el detalle de giro se diligencia por proyectos el cual tendra como hijo directo las fases y los criterios asociados a esa fase y al proyecto.
                    */
                    // Peticion asincrona de los proyectos por contratoId
                    const getProyectosByIdContrato: any[] = await this.registrarPagosSvc.getProyectosByIdContrato( this.solicitudPago.contratoId ).toPromise();
                    const LISTA_PROYECTOS: any[] = getProyectosByIdContrato[1];
                    const solicitudPagoFase: any[] = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[ 0 ].solicitudPagoFase;

                    LISTA_PROYECTOS.forEach( proyecto => {
                        // Objeto Proyecto que se agregara al array listaDetalleGiro
                        const PROYECTO = {
                            semaforoDetalle: 'sin-diligenciar',
                            contratacionProyectoId: proyecto.contratacionProyectoId,
                            llaveMen: proyecto.llaveMen,
                            fases: []
                        }

                        const listFase = solicitudPagoFase.filter( fase => fase.contratacionProyectoId === proyecto.contratacionProyectoId )
                        if ( listFase.length > 0 ) {
                            listFase.forEach( fase => {
                                fase.estadoSemaforo = 'sin-diligenciar'
                                fase.estadoSemaforoCausacion = 'sin-diligenciar'
                            })
                        }
                        PROYECTO.fases = listFase

                        if ( PROYECTO.fases.length > 0 ) {
                            this.listaDetalleGiro.push( PROYECTO )
                        }
                    } )

                    // Get Observaciones
                    this.getObservaciones()
                }
            );
    }

    ngOnInit(): void {
    }

    async getObservaciones(){
        // Request observaciones
        const listaObservacionVerificar = await this.obsOrdenGiro.getObservacionOrdenGiroByMenuIdAndSolicitudPagoId(
            this.listaMenu.verificarOrdenGiro,
            this.ordenGiroId,
            this.ordenGiroId,
            this.tipoObservaciones.terceroGiro );
        const listaObservacionAprobar = await this.obsOrdenGiro.getObservacionOrdenGiroByMenuIdAndSolicitudPagoId(
            this.listaMenu.aprobarOrdenGiro,
            this.ordenGiroId,
            this.ordenGiroId,
            this.tipoObservaciones.terceroGiro );
        const listaObservacionTramitar = await this.obsOrdenGiro.getObservacionOrdenGiroByMenuIdAndSolicitudPagoId(
                this.listaMenu.tramitarOrdenGiro,
                this.ordenGiroId,
                this.ordenGiroId,
                this.tipoObservaciones.terceroGiro );
        if ( listaObservacionVerificar.length > 0 ) {
            listaObservacionVerificar.forEach( obs => obs.menuId = this.listaMenu.verificarOrdenGiro );
        }
        if ( listaObservacionAprobar.length > 0 ) {
            listaObservacionAprobar.forEach( obs => obs.menuId = this.listaMenu.aprobarOrdenGiro );
        }
        if ( listaObservacionTramitar.length > 0 ) {
            listaObservacionTramitar.forEach( obs => obs.menuId = this.listaMenu.tramitarOrdenGiro )
        }
        // Get lista de observacion y observacion actual    
        const observacion = listaObservacionAprobar.find( obs => obs.archivada === false )

        if ( observacion !== undefined ) {
            this.ordenGiroObservacionId = observacion.ordenGiroObservacionId;

            this.formObservacion.setValue(
                {
                    tieneObservaciones: observacion.tieneObservacion,
                    observaciones: observacion.observacion !== undefined ? ( observacion.observacion.length > 0 ? observacion.observacion : null ) : null,
                    fechaCreacion: observacion.fechaCreacion
                }
            )
        }

        const obsArchivadasVerificar = listaObservacionVerificar.filter( obs => obs.archivada === true && obs.tieneObservacion === true );
        const obsArchivadasAprobar = listaObservacionAprobar.filter( obs => obs.archivada === true && obs.tieneObservacion === true );
        const obsArchivadasTramitar = listaObservacionTramitar.filter( obs => obs.archivada === true && obs.tieneObservacion === true );

        if ( obsArchivadasVerificar.length > 0 ) {
            obsArchivadasVerificar.forEach( obs => this.historialObservaciones.push( obs ) );
        }
        if ( obsArchivadasAprobar.length > 0 ) {
            obsArchivadasAprobar.forEach( obs => this.historialObservaciones.push( obs ) );
        }
        if ( obsArchivadasTramitar.length > 0 ) {
            obsArchivadasTramitar.forEach( obs => this.historialObservaciones.push( obs ) );
        }

        this.tablaHistorial = new MatTableDataSource( this.historialObservaciones );
    }

    maxLength(e: any, n: number) {
        if (e.editor.getLength() > n) {
            e.editor.deleteText(n - 1, e.editor.getLength());
        }
    }

    textoLimpio( evento: any, n: number ) {
        if ( evento !== undefined ) {
            return evento.getLength() > n ? n : evento.getLength();
        } else {
            return 0;
        }
    }

    getModalidadContrato( modalidadCodigo: string ) {
        if ( this.listaModalidadContrato.length > 0 ) {
            const modalidad = this.listaModalidadContrato.find( modalidad => modalidad.codigo === modalidadCodigo );
            
            if ( modalidad !== undefined ) {
                return modalidad.nombre;
            }
        }
    }

    checkSemaforoDetalle( listaSemaforosDetalle: any ) {
        const tieneSinDiligenciar = Object.values( listaSemaforosDetalle ).includes( 'sin-diligenciar' );
        const tieneEnProceso = Object.values( listaSemaforosDetalle ).includes( 'en-proceso' );
        const tieneCompleto = Object.values( listaSemaforosDetalle ).includes( 'completo' );

        if ( tieneEnProceso === true ) {
            this.semaforoDetalleGiro = 'en-proceso';
        }
        if ( tieneSinDiligenciar === true && tieneCompleto === true ) {
            this.semaforoDetalleGiro = 'en-proceso';
        }
        if ( tieneSinDiligenciar === false && tieneEnProceso === false && tieneCompleto === true ) {
            this.semaforoDetalleGiro = 'completo';
        }
    }

    openDialog( modalTitle: string, modalText: string ) {
        this.dialog.open( ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    guardar() {
        if ( this.formObservacion.get( 'tieneObservaciones' ).value === false && this.formObservacion.get( 'observaciones' ).value !== null ) {
            this.formObservacion.get( 'observaciones' ).setValue( '' );
        }

        const pOrdenGiroObservacion = {
            ordenGiroObservacionId: this.ordenGiroObservacionId,
            ordenGiroId: this.ordenGiroId,
            tipoObservacionCodigo: this.tipoObservaciones.terceroGiro,
            menuId: this.listaMenu.aprobarOrdenGiro,
            idPadre: this.ordenGiroId,
            observacion: this.formObservacion.get( 'observaciones' ).value,
            tieneObservacion: this.formObservacion.get( 'tieneObservaciones' ).value
        }

        this.obsOrdenGiro.createEditSpinOrderObservations( pOrdenGiroObservacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate(
                            [
                                this.esRegistroNuevo === true ? '/aprobarOrdenGiro/aprobarOrdenGiro' : '/aprobarOrdenGiro/editarOrdenGiro', this.solicitudPago.solicitudPagoId
                            ]
                        )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
