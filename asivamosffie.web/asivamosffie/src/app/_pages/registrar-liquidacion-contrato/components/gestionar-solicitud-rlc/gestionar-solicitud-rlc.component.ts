import { ActivatedRoute, UrlSegment, Router } from '@angular/router';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { FaseUnoPreconstruccionService } from 'src/app/core/_services/faseUnoPreconstruccion/fase-uno-preconstruccion.service';
import { TipoSolicitudCodigo } from 'src/app/_interfaces/estados-liquidacion-contratos.interface';
import { LiquidacionContratoService } from 'src/app/core/_services/liquidacionContrato/liquidacion-contrato.service';

@Component({
  selector: 'app-gestionar-solicitud-rlc',
  templateUrl: './gestionar-solicitud-rlc.component.html',
  styleUrls: ['./gestionar-solicitud-rlc.component.scss']
})
export class GestionarSolicitudRlcComponent implements OnInit, OnDestroy {

    estaEditando = false;
    esVerDetalle = false;
    tipoSolicitudCodigo = TipoSolicitudCodigo;
    contrato: any;
    realizoPeticion = false;
    addressForm = this.fb.group({
        fechaEnvioFirmaContratista: [ null, Validators.required ],
        fechaFirmaParteContratista: [ null, Validators.required ],
        fechaEnvioFirmaFiduciaria: [ null, Validators.required ],
        fechaFirmaParteFiduciaria: [ null, Validators.required ],
        observaciones: [ null, Validators.required ],
        urlSoporte : [ null, Validators.required ]
    });
    contratacionId = 0;
    editorStyle = {
        height: '50px'
    };
    config = {
        toolbar: [
            ['bold', 'italic', 'underline'],
            [{ list: 'ordered' }, { list: 'bullet' }],
            [{ indent: '-1' }, { indent: '+1' }],
            [{ align: [] }],
        ]
    };
    newDate = new Date();

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private activatedRoute: ActivatedRoute,
        private faseUnoPreconstruccionSvc: FaseUnoPreconstruccionService,
        private liquidacionContratoSvc: LiquidacionContratoService,
        private routes: Router )
    {
        this.activatedRoute.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
            if ( urlSegment.path === 'verDetalleEditarSolicitud' ) {
                this.estaEditando = true;
                return;
            }
            if ( urlSegment.path === 'verDetalleSolicitud' ) {
                this.esVerDetalle = true;
                return;
            }
        } );
        this.getContrato();
    }

    ngOnDestroy(): void {
        if ( this.addressForm.dirty === true && this.realizoPeticion === false ) {
            this.openDialogConfirmar( '', '<b>¿Desea guardar la información registrada?</b>' );
        }
    }

    ngOnInit(): void {
    }

    getContrato() {
        this.faseUnoPreconstruccionSvc.getContratacionByContratoId( this.activatedRoute.snapshot.params.id )
            .subscribe( getContratacionByContratoId => {
                this.contrato = getContratacionByContratoId;
                const contratacion = this.contrato.contratacion;
                this.contratacionId = this.contrato?.contratacionId;
                this.addressForm.setValue(
                    {
                        fechaEnvioFirmaContratista: contratacion.fechaFirmaEnvioContratista !== undefined ? contratacion.fechaFirmaEnvioContratista : null,
                        fechaFirmaParteContratista: contratacion.fechaFirmaContratista !== undefined ? contratacion.fechaFirmaContratista : null,
                        fechaEnvioFirmaFiduciaria: contratacion.fechaFirmaEnvioFiduciaria !== undefined ? contratacion.fechaFirmaEnvioFiduciaria : null,
                        fechaFirmaParteFiduciaria: contratacion.fechaFirmaFiduciaria !== undefined ? contratacion.fechaFirmaFiduciaria : null,
                        observaciones: contratacion.observacionesLiquidacion !== undefined ? contratacion.observacionesLiquidacion : null,
                        urlSoporte: contratacion.urlDocumentoLiquidacion !== undefined ? contratacion.urlDocumentoLiquidacion : null
                    }
                )
            } );
    }

    validateNumberKeypress(event: KeyboardEvent) {
        const alphanumeric = /[0-9]/;
        const inputChar = String.fromCharCode(event.charCode);
        return alphanumeric.test(inputChar) ? true : false;
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

    openDialog( modalTitle: string, modalText: string ) {
        this.dialog.open( ModalDialogComponent, {
            width: '28em',
            data: { modalTitle, modalText }
        } );
    }

    openDialogConfirmar( modalTitle: string, modalText: string ) {
        const confirmarDialog = this.dialog.open(ModalDialogComponent, {
          width: '30em',
          data: { modalTitle, modalText, siNoBoton: true }
        });

        confirmarDialog.afterClosed()
            .subscribe(
                response => {
                    if ( response === true ) {
                        this.onSubmit();
                    }
                }
            );
    }

    onSubmit() {
        this.estaEditando = true;
        const pContratacion = {
            contratacionId: this.contrato.contratacion.contratacionId,
            fechaFirmaEnvioContratista: this.addressForm.get( 'fechaEnvioFirmaContratista' ).value !== null ? new Date( this.addressForm.get( 'fechaEnvioFirmaContratista' ).value ).toISOString() : null,
            fechaFirmaContratista: this.addressForm.get( 'fechaFirmaParteContratista' ).value !== null ? new Date( this.addressForm.get( 'fechaFirmaParteContratista' ).value ).toISOString() : null,
            fechaFirmaEnvioFiduciaria: this.addressForm.get( 'fechaEnvioFirmaFiduciaria' ).value !== null ? new Date( this.addressForm.get( 'fechaEnvioFirmaFiduciaria' ).value ).toISOString() : null,
            fechaFirmaFiduciaria: this.addressForm.get( 'fechaFirmaParteFiduciaria' ).value !== null ? new Date( this.addressForm.get( 'fechaFirmaParteFiduciaria' ).value ).toISOString() : null,
            observacionesLiquidacion: this.addressForm.get( 'observaciones' ).value,
            urlDocumentoLiquidacion: this.addressForm.get( 'urlSoporte' ).value
        }

        this.liquidacionContratoSvc.createEditContractSettlement( pContratacion )
            .subscribe(
                response => {
                    this.realizoPeticion = true;
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate( [ '/registrarLiquidacionContrato' ] )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
