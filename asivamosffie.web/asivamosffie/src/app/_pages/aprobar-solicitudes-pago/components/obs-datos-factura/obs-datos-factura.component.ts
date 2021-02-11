import { CommonService } from './../../../../core/_services/common/common.service';
import { Component, Input, OnInit } from '@angular/core';
import { FormArray, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { Dominio } from 'src/app/core/_services/common/common.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';

@Component({
  selector: 'app-obs-datos-factura',
  templateUrl: './obs-datos-factura.component.html',
  styleUrls: ['./obs-datos-factura.component.scss']
})
export class ObsDatosFacturaComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle = false;
    @Input() aprobarSolicitudPagoId: any;
    @Input() datosFacturaCodigo: string;
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
        private commonSvc: CommonService,
        private dialog: MatDialog,
        private obsMultipleSvc: ObservacionesMultiplesCuService )
    {
        this.commonSvc.tiposDescuento()
            .subscribe( response => this.tiposDescuentoArray = response );
        this.addressForm = this.crearFormulario();
    }

    ngOnInit(): void {
      this.getDatosFactura();
      this.obsMultipleSvc.getObservacionSolicitudPagoByMenuIdAndSolicitudPagoId( this.aprobarSolicitudPagoId, this.solicitudPago.solicitudPagoId, this.solicitudPagoFaseFactura.solicitudPagoFaseFacturaId )
        .subscribe(
            response => {
                const obsSupervisor = response.filter( obs => obs.archivada === false )[0];

                if ( obsSupervisor !== undefined ) {
                    console.log( obsSupervisor );
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

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    onSubmit() {
      
        const pSolicitudPagoObservacion = {
            solicitudPagoObservacionId: this.solicitudPagoObservacionId,
            solicitudPagoId: this.solicitudPago.solicitudPagoId,
            observacion: this.addressForm.get( 'observaciones' ).value !== null ? this.addressForm.get( 'observaciones' ).value : this.addressForm.get( 'observaciones' ).value,
            tipoObservacionCodigo: this.datosFacturaCodigo,
            menuId: this.aprobarSolicitudPagoId,
            idPadre: this.solicitudPagoFaseFactura.solicitudPagoFaseFacturaId,
            tieneObservacion: this.addressForm.get( 'tieneObservaciones' ).value !== null ? this.addressForm.get( 'tieneObservaciones' ).value : this.addressForm.get( 'tieneObservaciones' ).value
        };

        console.log( pSolicitudPagoObservacion );
        this.obsMultipleSvc.createUpdateSolicitudPagoObservacion( pSolicitudPagoObservacion )
            .subscribe(
                response => this.openDialog( '', `<b>${ response.message }</b>` ),
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            )
    }

}
