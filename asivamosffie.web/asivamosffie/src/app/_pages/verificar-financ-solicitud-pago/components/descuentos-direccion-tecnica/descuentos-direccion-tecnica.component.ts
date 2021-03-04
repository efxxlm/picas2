import { Component, Input, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-descuentos-direccion-tecnica',
  templateUrl: './descuentos-direccion-tecnica.component.html',
  styleUrls: ['./descuentos-direccion-tecnica.component.scss']
})
export class DescuentosDireccionTecnicaComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle = false;
    @Input() aprobarSolicitudPagoId: any;
    @Input() datosFacturaDescuentoCodigo: string;
    solicitudPagoObservacionId = 0;
    formDescuentos: FormGroup;
    valorFacturado = 0;
    tiposDescuentoArray: Dominio[] = [];
    solicitudPagoFaseFacturaDescuento: any[] = [];
    solicitudPagoFaseFacturaId = 0;
    solicitudPagoFaseFactura: any;
    solicitudPagoFase: any;
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
        return this.formDescuentos.get( 'descuentos' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder,
        private commonSvc: CommonService )
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
                this.formDescuentos.get( 'numeroDescuentos' ).setValue( `${ this.solicitudPagoFaseFacturaDescuento.length }` );
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
    }

    crearFormulario() {
        this.formDescuentos = this.fb.group({
            aplicaDescuento: [ null],
            numeroDescuentos: [ '' ],
            valorAPagarDespues: [ { value: null, disabled: true } ],
            descuentos: this.fb.array( [] )
        });
    }

    getTipoDescuento( tipoDescuentoCodigo: string ) {
        if ( this.tiposDescuentoArray.length > 0 ) {
            const descuento = this.tiposDescuentoArray.filter( descuento => descuento.codigo === tipoDescuentoCodigo );
            return descuento[0].nombre;
        }
    }

}
