import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ActivatedRoute, Router } from '@angular/router';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';
import humanize from 'humanize-plus';

@Component({
  selector: 'app-obs-cargar-formpago-autoriz',
  templateUrl: './obs-cargar-formpago-autoriz.component.html',
  styleUrls: ['./obs-cargar-formpago-autoriz.component.scss']
})
export class ObsCargarFormpagoAutorizComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle = false;
    @Input() autorizarSolicitudPagoId: any;
    @Input() cargarFormaPagoCodigo: string;
    @Input() solicitudPagoCargarFormaPago: any;
    @Input() tieneFormaPago: boolean;
    @Output() estadoSemaforo = new EventEmitter<string>();
    solicitudPagoObservacionId = 0;
    addressForm: FormGroup;
    formaPagoArray: Dominio[] = [];
    editorStyle = {
      height: '45px',
      overflow: 'auto'
    };
    config = {
      toolbar: [
        ['bold', 'italic', 'underline'],
        [{ list: 'ordered' }, { list: 'bullet' }],
        [{ indent: '-1' }, { indent: '+1' }],
        [{ align: [] }],
      ]
    };
    estaEditando = true;

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private routes: Router,
        private activatedRoute: ActivatedRoute,
        private obsMultipleSvc: ObservacionesMultiplesCuService,
        private commonSvc: CommonService )
    {
        this.addressForm = this.crearFormulario();
        this.commonSvc.formasDePago()
            .subscribe( response => this.formaPagoArray = response );
    }

    ngOnInit(): void {
        if ( this.solicitudPago !== undefined ) {
            if ( this.tieneFormaPago === true ) {
                this.obsMultipleSvc.getObservacionSolicitudPagoByMenuIdAndSolicitudPagoId( this.autorizarSolicitudPagoId, this.solicitudPago.solicitudPagoId, this.solicitudPagoCargarFormaPago.solicitudPagoCargarFormaPagoId )
                    .subscribe(
                        response => {
                            const obsSupervisor = response.filter( obs => obs.archivada === false )[0];

                            if ( obsSupervisor !== undefined ) {

                                if ( obsSupervisor.registroCompleto === false ) {
                                    this.estadoSemaforo.emit( 'en-proceso' );
                                }
                                if ( obsSupervisor.registroCompleto === true ) {
                                    this.estadoSemaforo.emit( 'completo' );
                                }
                                console.log( obsSupervisor );
                                this.estaEditando = true;
                                this.addressForm.markAllAsTouched();
                                this.solicitudPagoObservacionId = obsSupervisor.solicitudPagoObservacionId;
                                this.addressForm.setValue(
                                    {
                                        fechaCreacion: obsSupervisor.fechaCreacion,
                                        tieneObservaciones: obsSupervisor.tieneObservacion !== undefined ? obsSupervisor.tieneObservacion : null,
                                        observaciones: obsSupervisor.observacion !== undefined ? ( obsSupervisor.observacion.length > 0 ? obsSupervisor.observacion : null ) : null
                                    }
                                );
                            }
                        }
                    );
            }
            if ( this.tieneFormaPago === false ) {
                this.estadoSemaforo.emit( 'completo' );
            }
        }
    }

    crearFormulario() {
        return this.fb.group({
            fechaCreacion: [ null ],
            tieneObservaciones: [null, Validators.required],
            observaciones:[null, Validators.required],
        })
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


    firstLetterUpperCase( texto:string ) {
        if ( texto !== undefined ) {
            return humanize.capitalize( String( texto ).toLowerCase() );
        }
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    getFormaPago( formaPagoCodigo: string ) {
        if ( this.formaPagoArray.length > 0 && formaPagoCodigo !== undefined ) {
            const forma = this.formaPagoArray.filter( forma => forma.codigo === formaPagoCodigo );
            if ( forma.length > 0 ) {
                return forma[0].nombre;
            }
        }
    }

    onSubmit() {
        this.estaEditando = true;
        this.addressForm.markAllAsTouched();
        if ( this.addressForm.get( 'tieneObservaciones' ).value !== null && this.addressForm.get( 'tieneObservaciones' ).value === false ) {
            this.addressForm.get( 'observaciones' ).setValue( '' );
        }

        const pSolicitudPagoObservacion = {
            solicitudPagoObservacionId: this.solicitudPagoObservacionId,
            solicitudPagoId: this.solicitudPago.solicitudPagoId,
            observacion: this.addressForm.get( 'observaciones' ).value !== null ? this.addressForm.get( 'observaciones' ).value : this.addressForm.get( 'observaciones' ).value,
            tipoObservacionCodigo: this.cargarFormaPagoCodigo,
            menuId: this.autorizarSolicitudPagoId,
            idPadre: this.solicitudPagoCargarFormaPago.solicitudPagoCargarFormaPagoId,
            tieneObservacion: this.addressForm.get( 'tieneObservaciones' ).value !== null ? this.addressForm.get( 'tieneObservaciones' ).value : this.addressForm.get( 'tieneObservaciones' ).value
        };

        this.obsMultipleSvc.createUpdateSolicitudPagoObservacion( pSolicitudPagoObservacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate(
                            [
                                '/autorizarSolicitudPago/autorizacionSolicitud',  this.activatedRoute.snapshot.params.idContrato, this.activatedRoute.snapshot.params.idSolicitudPago
                            ]
                        )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            )
    }

}
