import { ListaMediosPagoCodigo, MediosPagoCodigo } from './../../../../_interfaces/estados-solicitudPago-ordenGiro.interface';
import { Router } from '@angular/router';
import { OrdenPagoService } from 'src/app/core/_services/ordenPago/orden-pago.service';
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-tercero-giro-gog',
  templateUrl: './form-tercero-giro-gog.component.html',
  styleUrls: ['./form-tercero-giro-gog.component.scss']
})
export class FormTerceroGiroGogComponent implements OnInit {

    @Input() solicitudPago: any;
    medioPagoArray: Dominio[] = [];
    bancosArray: Dominio[] = [];
    addressForm: FormGroup;
    listaMediosPagoCodigo: ListaMediosPagoCodigo = MediosPagoCodigo;
    ordenGiroId = 0;
    ordenGiroTerceroId = 0;
    estaEditando = false;
    constructor(
        private fb: FormBuilder,
        private commonSvc: CommonService,
        private ordenPagoSvc: OrdenPagoService,
        private dialog: MatDialog,
        private routes: Router )
    {
        this.addressForm = this.crearFormulario();
        this.commonSvc.listaMediosPago()
            .subscribe( response => this.medioPagoArray = response );
        this.commonSvc.listaBancos()
            .subscribe( response => this.bancosArray = response );
    }

    ngOnInit(): void {
        if ( this.solicitudPago.ordenGiro !== undefined ) {
            this.ordenGiroId = this.solicitudPago.ordenGiro.ordenGiroId;

            // if (  )
        }
    }

    crearFormulario() {
        return this.fb.group({
            medioPagoGiroContrato: [null, Validators.required],
            transferenciaElectronica: this.fb.group( {
                ordenGiroTerceroTransferenciaElectronicaId: [ 0 ],
                titularCuenta: [ '' ],
                titularNumeroIdentificacion: [ '' ],
                numeroCuenta: [ '' ],
                bancoCodigo: [ null ],
                esCuentaAhorros: [ null ]
            } ),
            chequeGerencia: this.fb.group( {
                ordenGiroTerceroChequeGerenciaId: [ 0 ],
                nombreBeneficiario: [ '' ],
                numeroIdentificacionBeneficiario: [ '' ]
            } )
        })
    }

    maxLength(e: any, n: number) {
      console.log(e.editor.getLength()+" "+n);
      if (e.editor.getLength() > n) {
        e.editor.deleteText(n-1, e.editor.getLength());
      }
    }

    textoLimpio(texto,n) {
      if (texto!=undefined) {
        return texto.getLength() > n ? n : texto.getLength();
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
        const ordenGiroTerceroDiligenciado = () => {
            if (this.listaMediosPagoCodigo.transferenciaElectronica === this.addressForm.get( 'medioPagoGiroContrato' ).value ) {
                return this.addressForm.get( 'transferenciaElectronica' ).value;
            }

            if ( this.listaMediosPagoCodigo.chequeGerencia === this.addressForm.get( 'medioPagoGiroContrato' ).value ) {
                return this.addressForm.get( 'chequeGerencia' ).value;
            }
        }

        const pOrdenGiro = {
            solicitudPagoId: this.solicitudPago.solicitudPagoId,
            ordenGiroId: this.ordenGiroId,
            ordenGiroTercero: {
                ordenGiroTerceroId: this.ordenGiroTerceroId,
                medioPagoGiroCodigo: this.addressForm.get( 'medioPagoGiroContrato' ).value,
                [
                    this.listaMediosPagoCodigo.transferenciaElectronica === this.addressForm.get( 'medioPagoGiroContrato' ).value ? 'ordenGiroTerceroTransferenciaElectronica' : 'ordenGiroTerceroChequeGerencia'
                ]: ordenGiroTerceroDiligenciado()
            }
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
