import { ActualizarPolizasService } from 'src/app/core/_services/actualizarPolizas/actualizar-polizas.service';
import { Component, Input, OnInit } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-lista-chequeo-rapg',
  templateUrl: './lista-chequeo-rapg.component.html',
  styleUrls: ['./lista-chequeo-rapg.component.scss']
})
export class ListaChequeoRapgComponent implements OnInit {

    @Input() contratoPoliza: any;
    @Input() esVerDetalle: boolean;
    contratoPolizaActualizacion: any;
    contratoPolizaActualizacionListaChequeo: any;
    addressForm = this.fb.group( {
        contratoPolizaActualizacionId: [ 0 ],
        contratoPolizaActualizacionListaChequeoId: [ 0 ],
        cumpleAsegurado: [null, Validators.required],
        cumpleBeneficiario: [null, Validators.required],
        cumpleAfianzado: [null, Validators.required],
        reciboDePago: [null, Validators.required],
        condicionesGenerales: [null, Validators.required]
    } );
    estaEditando = false;

    constructor(
        private fb: FormBuilder,
        private routes: Router,
        private dialog: MatDialog,
        private actualizarPolizaSvc: ActualizarPolizasService )
    { }

    ngOnInit(): void {
        this.getListaChequeo();
    }

    getListaChequeo() {
        if ( this.contratoPoliza.contratoPolizaActualizacion !== undefined ) {
            if ( this.contratoPoliza.contratoPolizaActualizacion.length > 0 ) {
                this.contratoPolizaActualizacion = this.contratoPoliza.contratoPolizaActualizacion[ 0 ];
                this.addressForm.get( 'contratoPolizaActualizacionId' ).setValue( this.contratoPolizaActualizacion.contratoPolizaActualizacionId );

                if ( this.contratoPolizaActualizacion.contratoPolizaActualizacionListaChequeo !== undefined ) {
                    if ( this.contratoPolizaActualizacion.contratoPolizaActualizacionListaChequeo.length > 0 ) {
                        this.contratoPolizaActualizacionListaChequeo = this.contratoPolizaActualizacion.contratoPolizaActualizacionListaChequeo[ 0 ];

                        this.addressForm.get( 'contratoPolizaActualizacionListaChequeoId' ).setValue( this.contratoPolizaActualizacionListaChequeo.contratoPolizaActualizacionListaChequeoId );
                        this.addressForm.get( 'cumpleAsegurado' ).setValue( this.contratoPolizaActualizacionListaChequeo.cumpleDatosAseguradoBeneficiario !== undefined ? this.contratoPolizaActualizacionListaChequeo.cumpleDatosAseguradoBeneficiario : null );
                        this.addressForm.get( 'cumpleBeneficiario' ).setValue( this.contratoPolizaActualizacionListaChequeo.cumpleDatosBeneficiarioGarantiaBancaria !== undefined ? this.contratoPolizaActualizacionListaChequeo.cumpleDatosBeneficiarioGarantiaBancaria : null );
                        this.addressForm.get( 'cumpleAfianzado' ).setValue( this.contratoPolizaActualizacionListaChequeo.cumpleDatosTomadorAfianzado !== undefined ? this.contratoPolizaActualizacionListaChequeo.cumpleDatosTomadorAfianzado : null );
                        this.addressForm.get( 'reciboDePago' ).setValue( this.contratoPolizaActualizacionListaChequeo.tieneReciboPagoDatosRequeridos !== undefined ? this.contratoPolizaActualizacionListaChequeo.tieneReciboPagoDatosRequeridos : null );
                        this.addressForm.get( 'condicionesGenerales' ).setValue( this.contratoPolizaActualizacionListaChequeo.tieneCondicionesGeneralesPoliza !== undefined ? this.contratoPolizaActualizacionListaChequeo.tieneCondicionesGeneralesPoliza : null );
                    }
                }
            }
        }
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open( ModalDialogComponent, {
            width: '28em',
            data: { modalTitle, modalText }
        });
    }

    onSubmit() {
        this.estaEditando = true;

        const getContratoPolizaActualizacionListaChequeo = () => {
            return [
                {
                    contratoPolizaActualizacionListaChequeoId: this.addressForm.get( 'contratoPolizaActualizacionListaChequeoId' ).value,
                    contratoPolizaActualizacionId: this.addressForm.get( 'contratoPolizaActualizacionId' ).value,
                    cumpleDatosAseguradoBeneficiario: this.addressForm.get( 'cumpleAsegurado' ).value,
                    cumpleDatosBeneficiarioGarantiaBancaria: this.addressForm.get( 'cumpleBeneficiario' ).value,
                    cumpleDatosTomadorAfianzado: this.addressForm.get( 'cumpleAfianzado' ).value,
                    tieneReciboPagoDatosRequeridos: this.addressForm.get( 'reciboDePago' ).value,
                    tieneCondicionesGeneralesPoliza: this.addressForm.get( 'condicionesGenerales' ).value
                }
            ]
        }

        this.contratoPolizaActualizacion.contratoPolizaActualizacionListaChequeo = getContratoPolizaActualizacionListaChequeo()

        this.actualizarPolizaSvc.createorUpdateCofinancing( this.contratoPolizaActualizacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate(
                            [
                                '/registrarActualizacionesPolizasYGarantias/verDetalleEditarPoliza', this.contratoPoliza.contratoPolizaId
                            ]
                        )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
