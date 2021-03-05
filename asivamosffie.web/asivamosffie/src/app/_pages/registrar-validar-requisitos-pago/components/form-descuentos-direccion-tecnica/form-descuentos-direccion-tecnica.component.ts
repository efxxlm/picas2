import { Component, Input, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-descuentos-direccion-tecnica',
  templateUrl: './form-descuentos-direccion-tecnica.component.html',
  styleUrls: ['./form-descuentos-direccion-tecnica.component.scss']
})
export class FormDescuentosDireccionTecnicaComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle = false;
    formDescuentos: FormGroup;
    valorFacturado = 0;
    tiposDescuentoArray: Dominio[] = [];
    solicitudPagoFaseFacturaDescuento: any[] = [];
    solicitudPagoFaseFacturaId = 0;
    solicitudPagoFaseFactura: any;
    solicitudPagoFase: any;

    get descuentos() {
        return this.formDescuentos.get( 'descuentos' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private commonSvc: CommonService,
        private routes: Router,
        private registrarPagosSvc: RegistrarRequisitosPagoService )
    {
        this.commonSvc.tiposDescuento()
            .subscribe( response => this.tiposDescuentoArray = response );
        this.crearFormulario();
    }

    ngOnInit(): void {
        this.getDatosFactura();
    }

    getDatosFactura() {
        this.solicitudPagoFase = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0];
        this.solicitudPagoFaseFactura = this.solicitudPagoFase.solicitudPagoFaseFactura[0];
        if ( this.solicitudPagoFaseFactura !== undefined ) {
            this.solicitudPagoFaseFacturaId = this.solicitudPagoFaseFactura.solicitudPagoFaseFacturaId;
            this.solicitudPagoFaseFacturaDescuento = this.solicitudPagoFaseFactura.solicitudPagoFaseFacturaDescuento;

            if ( this.solicitudPagoFaseFacturaDescuento !== null ) {
                this.formDescuentos.get( 'aplicaDescuento' ).setValue( this.solicitudPagoFaseFactura.tieneDescuento !== undefined ? this.solicitudPagoFaseFactura.tieneDescuento : null );
                this.formDescuentos.get( 'numeroDescuentos' ).setValue( this.solicitudPagoFaseFacturaDescuento.length > 0 ? `${ this.solicitudPagoFaseFacturaDescuento.length }` : '' );
                this.formDescuentos.get( 'valorAPagarDespues' ).setValue( this.solicitudPagoFaseFactura.valorFacturadoConDescuento !== undefined ? this.solicitudPagoFaseFactura.valorFacturadoConDescuento : null );
                for ( const descuento of this.solicitudPagoFaseFacturaDescuento ) {
                    this.descuentos.push(
                        this.fb.group(
                            {
                                solicitudPagoFaseFacturaDescuentoId: [ descuento.solicitudPagoFaseFacturaDescuentoId ],
                                solicitudPagoFaseFacturaId: [ descuento.solicitudPagoFaseFacturaId ],
                                tipoDescuentoCodigo: [ descuento.tipoDescuentoCodigo ],
                                valorDescuento: [ descuento.valorDescuento ]
                            }
                        )
                    );
                }
            }
        }
        for ( const criterio of this.solicitudPagoFase.solicitudPagoFaseCriterio ) {
            this.valorFacturado += criterio.valorFacturado;
        }
        this.formDescuentos.get( 'numeroDescuentos' ).valueChanges
            .subscribe(
                value => {
                    value = Number( value );
                    if ( this.solicitudPagoFaseFactura !== undefined && this.solicitudPagoFaseFacturaDescuento.length > 0 ) {
                        if ( value > 0 ) {
                            this.descuentos.clear();
                            for ( const descuento of this.solicitudPagoFaseFacturaDescuento ) {
                                this.descuentos.push(
                                    this.fb.group(
                                        {
                                            solicitudPagoFaseFacturaDescuentoId: [ descuento.solicitudPagoFaseFacturaDescuentoId ],
                                            solicitudPagoFaseFacturaId: [ descuento.solicitudPagoFaseFacturaId ],
                                            tipoDescuentoCodigo: [ descuento.tipoDescuentoCodigo ],
                                            valorDescuento: [ descuento.valorDescuento ]
                                        }
                                    )
                                );
                            }

                            this.formDescuentos.get( 'numeroDescuentos' ).setValidators( Validators.min( this.descuentos.length ) );
                            const nuevosDescuentos = value - this.descuentos.length;
                            if ( value < this.descuentos.length ) {
                              this.openDialog(
                                '', '<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>'
                              );
                              this.formDescuentos.get( 'numeroDescuentos' ).setValue( String( this.descuentos.length ) );
                              return;
                            }
                            for ( let i = 0; i < nuevosDescuentos; i++ ) {
                              this.descuentos.push(
                                this.fb.group(
                                    {
                                        solicitudPagoFaseFacturaDescuentoId: [ 0 ],
                                        solicitudPagoFaseFacturaId: [ this.solicitudPagoFaseFacturaId ],
                                        tipoDescuentoCodigo: [ null ],
                                        valorDescuento: [ null ]
                                    }
                                )
                              );
                            }
                        }
                    } else {
                        if ( value > 0 ) {
                            if ( this.descuentos.dirty === true ) {
                                this.formDescuentos.get( 'numeroDescuentos' ).setValidators( Validators.min( this.descuentos.length ) );
                                const nuevosDescuentos = value - this.descuentos.length;
                                if ( value < this.descuentos.length ) {
                                  this.openDialog(
                                    '', '<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>'
                                  );
                                  this.formDescuentos.get( 'numeroDescuentos' ).setValue( String( this.descuentos.length ) );
                                  return;
                                }
                                for ( let i = 0; i < nuevosDescuentos; i++ ) {
                                  this.descuentos.push(
                                    this.fb.group(
                                        {
                                            solicitudPagoFaseFacturaDescuentoId: [ 0 ],
                                            solicitudPagoFaseFacturaId: [ this.solicitudPagoFaseFacturaId ],
                                            tipoDescuentoCodigo: [ null ],
                                            valorDescuento: [ null ]
                                        }
                                    )
                                  );
                                }
                            }
                            if ( this.descuentos.dirty === false ) {
                                this.descuentos.clear();
                                for ( let i = 0; i < value; i++ ) {
                                    this.descuentos.push(
                                        this.fb.group(
                                            {
                                                solicitudPagoFaseFacturaDescuentoId: [ 0 ],
                                                solicitudPagoFaseFacturaId: [ this.solicitudPagoFaseFacturaId ],
                                                tipoDescuentoCodigo: [ null ],
                                                valorDescuento: [ null ]
                                            }
                                        )
                                    );
                                }
                            }
                        }
                    }
                }
            );
    }

    crearFormulario () {
      this.formDescuentos = this.fb.group({
        aplicaDescuento: [ null ],
        numeroDescuentos: [ '' ],
        valorAPagarDespues: [ { value: null, disabled: true } ],
        descuentos: this.fb.array( [] )
      });
    }

    validateNumberKeypress(event: KeyboardEvent) {
      const alphanumeric = /[0-9]/;
      const inputChar = String.fromCharCode(event.charCode);
      return alphanumeric.test(inputChar) ? true : false;
    }

    validateNumber( value: any ) {
        if ( isNaN( Number( value ) ) === true ) {
            this.formDescuentos.get( 'numeroDescuentos' ).setValue( '' );
        }
    }

    disabledBtn() {
        if ( this.formDescuentos.get( 'aplicaDescuento' ).value === null ) {
            return true;
        }
        if ( this.formDescuentos.get( 'aplicaDescuento' ).value === true && this.descuentos.dirty === false ) {
            return true;
        }

        return false;
    }

    getTipoDescuento( tipoDescuentoCodigo: string ) {
        if ( this.tiposDescuentoArray.length > 0 ) {
            const descuento = this.tiposDescuentoArray.filter( descuento => descuento.codigo === tipoDescuentoCodigo );
            return descuento[0].nombre;
        }
    }

    totalPagoDescuentos() {
        let totalDescuentos = 0;
        let valorDespuesDescuentos = 0;
        this.descuentos.controls.forEach( control => {
            if ( control.value.valorDescuento !== null ) {
                totalDescuentos += control.value.valorDescuento;
            }
        } );
        if ( totalDescuentos > 0 ) {
            if ( totalDescuentos > this.valorFacturado ) {
                this.openDialog( '', '<b>El valor total de los descuentos es mayor al valor facturado.</b>' );
                this.formDescuentos.get( 'valorAPagarDespues' ).setValue( null );
                return;
            } else {
                valorDespuesDescuentos = this.valorFacturado - totalDescuentos;
                this.formDescuentos.get( 'valorAPagarDespues' ).setValue( valorDespuesDescuentos );
                return;
            }
        } else {
            this.formDescuentos.get( 'valorAPagarDespues' ).setValue( null );
            return;
        }
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    openDialogTrueFalse(modalTitle: string, modalText: string) {

        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText, siNoBoton: true }
        });

        return dialogRef.afterClosed();
    }

    addDescuento() {
        this.descuentos.push(
            this.fb.group(
                {
                    solicitudPagoFaseFacturaDescuentoId: [ 0 ],
                    solicitudPagoFaseFacturaId: [ this.solicitudPagoFaseFacturaId ],
                    tipoDescuentoCodigo: [ null ],
                    valorDescuento: [ null ]
                }
            )
        );
        this.formDescuentos.get( 'numeroDescuentos' ).setValidators( Validators.min( this.descuentos.length ) );
        this.formDescuentos.get( 'numeroDescuentos' ).setValue( String( this.descuentos.length ) );
    }

    deleteDescuento( index: number, descuentoId: number ) {
        this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
            .subscribe(
                response => {
                    if ( response === true ) {
                        if ( descuentoId === 0 ) {
                            this.descuentos.removeAt( index );
                            this.formDescuentos.get( 'numeroDescuentos' ).setValue( `${ this.descuentos.length }` );
                            this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
                        } else {
                            this.registrarPagosSvc.deleteSolicitudPagoFaseFacturaDescuento( descuentoId )
                                .subscribe(
                                    () => {
                                        this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
                                        this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                                            () => this.routes.navigate(
                                                [
                                                    '/registrarValidarRequisitosPago/verDetalleEditar', this.solicitudPago.contratoId, this.solicitudPago.solicitudPagoId
                                                ]
                                            )
                                        );
                                    },
                                    err => this.openDialog( '', `<b>${ err.message }</b>` )
                                );
                        }
                    }
                }
            );
    }

    guardar () {
        const getSolicitudPagoFaseFacturaDescuento = () => {
            if ( this.descuentos.length > 0 ) {
                if ( this.formDescuentos.get( 'aplicaDescuento' ).value === true ) {
                    return this.descuentos.value;
                } else {
                    return [];
                }
            } else {
                return [];
            }
        }

        const solicitudPagoFaseFactura = [
            {
                solicitudPagoFaseFacturaId: this.solicitudPagoFaseFacturaId,
                solicitudPagoFaseId: this.solicitudPagoFase.solicitudPagoFaseId,
                fecha: this.solicitudPagoFaseFactura.fecha,
                valorFacturado: this.valorFacturado,
                numero: this.solicitudPagoFaseFactura.numero,
                tieneDescuento: this.formDescuentos.get( 'aplicaDescuento' ).value,
                valorFacturadoConDescuento: this.formDescuentos.get( 'valorAPagarDespues' ).value,
                solicitudPagoFaseFacturaDescuento: getSolicitudPagoFaseFacturaDescuento()
            }
        ]
        console.log( solicitudPagoFaseFactura );
        this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0].solicitudPagoFaseFactura = solicitudPagoFaseFactura;
        this.registrarPagosSvc.createEditNewPayment( this.solicitudPago )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate(
                            [
                                '/registrarValidarRequisitosPago/verDetalleEditar', this.solicitudPago.contratoId, this.solicitudPago.solicitudPagoId
                            ]
                        )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
