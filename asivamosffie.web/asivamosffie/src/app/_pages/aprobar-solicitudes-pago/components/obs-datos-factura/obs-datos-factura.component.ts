import { CommonService } from './../../../../core/_services/common/common.service';
import { Component, Input, OnInit } from '@angular/core';
import { FormArray, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { Dominio } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-obs-datos-factura',
  templateUrl: './obs-datos-factura.component.html',
  styleUrls: ['./obs-datos-factura.component.scss']
})
export class ObsDatosFacturaComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle = false;
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
        this.solicitudPagoFase = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0];
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

    onSubmit() {
      console.log(this.addressForm.value);
    }

}
