import { Component, Input, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-datos-factura',
  templateUrl: './datos-factura.component.html',
  styleUrls: ['./datos-factura.component.scss']
})
export class DatosFacturaComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle = false;
    @Input() aprobarSolicitudPagoId: any;
    @Input() datosFacturaCodigo: string;
    @Input() esPreconstruccion = true;
    solicitudPagoRegistrarSolicitudPago: any;
    solicitudPagoObservacionId = 0;
    detalleForm = this.fb.group({
        numeroFactura: [null, Validators.required],
        fechaFactura: [null, Validators.required],
        aplicaDescuento: [null, Validators.required],
        numeroDescuentos: [ '' ],
        descuentos: this.fb.array( [] ),
        valorAPagarDespues: [null, Validators.required]
    });
    valorFacturado = 0;
    tiposDescuentoArray: Dominio[] = [];
    solicitudPagoFaseFacturaDescuento: any[] = [];
    solicitudPagoFaseFacturaId = 0;
    solicitudPagoFaseFactura: any;
    solicitudPagoFase: any;
    addressForm: FormGroup;

    get descuentos() {
        return this.detalleForm.get( 'descuentos' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder,
        private commonSvc: CommonService )
    {
        this.commonSvc.tiposDescuento()
            .subscribe( response => this.tiposDescuentoArray = response );
        this.addressForm = this.crearFormulario();
    }

    ngOnInit(): void {
      this.getDatosFactura();
    }

    getDatosFactura() {
        this.solicitudPagoRegistrarSolicitudPago = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0];
        if ( this.esPreconstruccion === true ) {
            if ( this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase !== undefined ) {
                if ( this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase.length > 0 ) {
                    for ( const solicitudPagoFase of this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase ) {
                        if ( solicitudPagoFase.esPreconstruccion === true ) {
                            this.solicitudPagoFase = solicitudPagoFase;
                        }
                    }
                }
            }
        }
        if ( this.esPreconstruccion === false ) {
            if ( this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase !== undefined ) {
                if ( this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase.length > 0 ) {
                    for ( const solicitudPagoFase of this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase ) {
                        if ( solicitudPagoFase.esPreconstruccion === false ) {
                            this.solicitudPagoFase = solicitudPagoFase;
                        }
                    }
                }
            }
        }

        this.solicitudPagoFaseFactura = this.solicitudPagoFase.solicitudPagoFaseFactura[0];
        if ( this.solicitudPagoFaseFactura !== undefined ) {
            this.solicitudPagoFaseFacturaId = this.solicitudPagoFaseFactura.solicitudPagoFaseFacturaId;
            this.solicitudPagoFaseFacturaDescuento = this.solicitudPagoFaseFactura.solicitudPagoFaseFacturaDescuento;
            this.detalleForm.get( 'numeroFactura' ).setValue( this.solicitudPagoFaseFactura.numero !== undefined ? this.solicitudPagoFaseFactura.numero : null );
            this.detalleForm.get( 'fechaFactura' ).setValue( this.solicitudPagoFaseFactura.fecha !== undefined ? new Date( this.solicitudPagoFaseFactura.fecha ) : null );
            this.detalleForm.get( 'aplicaDescuento' ).setValue( this.solicitudPagoFaseFactura.tieneDescuento !== undefined ? this.solicitudPagoFaseFactura.tieneDescuento : null );
            this.detalleForm.get( 'numeroDescuentos' ).setValue( `${ this.solicitudPagoFaseFacturaDescuento.length }` );
            this.detalleForm.get( 'valorAPagarDespues' ).setValue( this.solicitudPagoFaseFactura.valorFacturadoConDescuento !== undefined ? this.solicitudPagoFaseFactura.valorFacturadoConDescuento : null );
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
        for ( const criterio of this.solicitudPagoFase.solicitudPagoFaseCriterio ) {
            this.valorFacturado += criterio.valorFacturado;
        }
    }

    crearFormulario() {
        return this.fb.group({
            fechaCreacion: [ null ],
            tieneObservaciones: [null, Validators.required],
            observaciones:[null, Validators.required],
        })
    }

    getTipoDescuento( tipoDescuentoCodigo: string ) {
        if ( this.tiposDescuentoArray.length > 0 ) {
            const descuento = this.tiposDescuentoArray.filter( descuento => descuento.codigo === tipoDescuentoCodigo );
            return descuento[0].nombre;
        }
    }

}
