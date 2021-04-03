import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { OrdenPagoService } from 'src/app/core/_services/ordenPago/orden-pago.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-observaciones-orden-giro-gog',
  templateUrl: './observaciones-orden-giro-gog.component.html',
  styleUrls: ['./observaciones-orden-giro-gog.component.scss']
})
export class ObservacionesOrdenGiroGogComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle: boolean;
    ordenGiroId = 0;
    ordenGiroDetalleId = 0;
    ordenGiroObservacionId = 0;
    ordenGiroDetalle: any;
    addressForm = this.fb.group({});
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
    estaEditando = false;
    constructor(
        private fb: FormBuilder,
        private commonSvc: CommonService,
        private ordenPagoSvc: OrdenPagoService,
        private dialog: MatDialog,
        private routes: Router )
    {
        this.addressForm = this.crearFormulario();
    }

    ngOnInit(): void {
        if ( this.solicitudPago.ordenGiro !== undefined ) {
            this.ordenGiroId = this.solicitudPago.ordenGiro.ordenGiroId;
            
            if ( this.solicitudPago.ordenGiro.ordenGiroDetalle !== undefined ) {
                if ( this.solicitudPago.ordenGiro.ordenGiroDetalle.length > 0 ) {
                    this.ordenGiroDetalle = this.solicitudPago.ordenGiro.ordenGiroDetalle[0];
                    this.ordenGiroDetalleId = this.ordenGiroDetalle.ordenGiroDetalleId;
    
                    if ( this.ordenGiroDetalle.ordenGiroDetalleObservacion !== undefined ) {
                        if ( this.ordenGiroDetalle.ordenGiroDetalleObservacion.length > 0 ) {
                            this.ordenGiroObservacionId = this.ordenGiroDetalle.ordenGiroDetalleObservacion[0].ordenGiroObservacionId;

                            this.addressForm.setValue(
                                {
                                    observaciones: this.ordenGiroDetalle.ordenGiroDetalleObservacion[0].observacion !== undefined ? this.ordenGiroDetalle.ordenGiroDetalleObservacion[0].observacion : null
                                }
                            )
                        }
                    }
                }
            }
        }
    }

    crearFormulario() {
      return this.fb.group({
        observaciones:[null, Validators.required]
      })
    }

    maxLength( e: any, n: number ) {
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
        this.estaEditando = true;
        this.addressForm.markAllAsTouched();
        const pOrdenGiro = {
            solicitudPagoId: this.solicitudPago.solicitudPagoId,
            ordenGiroId: this.ordenGiroId,
            ordenGiroDetalle: [
                {
                    ordenGiroId: this.ordenGiroId,
                    ordenGiroDetalleId: this.ordenGiroDetalleId,
                    ordenGiroDetalleObservacion: [
                        {
                            ordenGiroDetalleId: this.ordenGiroDetalleId,
                            ordenGiroObservacionId: this.ordenGiroObservacionId,
                            observacion: this.addressForm.get( 'observaciones' ).value
                        }
                    ]
                }
            ]
        }

        this.ordenPagoSvc.createEditOrdenGiro( pOrdenGiro )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate(
                            [
                                '/generarOrdenDeGiro/generacionOrdenGiro', this.solicitudPago.solicitudPagoId
                            ]
                        )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
