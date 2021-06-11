import { ɵNullViewportScroller } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { FinancialBalanceService } from 'src/app/core/_services/financialBalance/financial-balance.service';
import { Respuesta } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-traslado-recursos-gbftrec',
  templateUrl: './traslado-recursos-gbftrec.component.html',
  styleUrls: ['./traslado-recursos-gbftrec.component.scss']
})
export class TrasladoRecursosGbftrecComponent implements OnInit {
  @Input() id: number;
  @Input() esVerDetalle: boolean;
  balanceFinanciero: any;
  balanceFinancieroId = 0;
  balanceFinancieroTraslado = [];
  trasladoPendiente = false;
  addressForm = this.fb.group({
    balanceFinancieroId: [null, Validators.required],
    proyectoId: [ɵNullViewportScroller, Validators.required],
    requiereTransladoRecursos: [null, Validators.required],
    justificacionTrasladoAportanteFuente: [null, Validators.required],
    urlSoporte: [null, Validators.required],
    fechaCreacion: [null, Validators.required],
  });
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
  estaEditando = false;
  constructor(private fb: FormBuilder, public dialog: MatDialog,private financialBalanceService: FinancialBalanceService,
    ) { }
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  maxLength(e: any, n: number) {
    
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
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  ngOnInit(): void {
    this.buildForm();
  }

  buildForm() {
    this.financialBalanceService.getBalanceFinanciero(
      this.id
    ).subscribe(response => {
      if(response != null){
        this.balanceFinanciero = response;

        if ( this.balanceFinanciero.balanceFinancieroTraslado !== undefined ) {
          if ( this.balanceFinanciero.balanceFinancieroTraslado.length > 0 ) {
            this.balanceFinancieroTraslado = this.balanceFinanciero.balanceFinancieroTraslado

            const trasladoIncompleto = this.balanceFinancieroTraslado.find( traslado => traslado.registroCompleto === false )

            if ( trasladoIncompleto !== undefined ) {
              this.trasladoPendiente = true;
            }
          }
        }
        

        this.balanceFinancieroId = response[ 'balanceFinancieroId' ]
        this.addressForm.patchValue(response);
      }
    });
  }

  onSubmit() {
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();

    if ( this.addressForm.get( 'requiereTransladoRecursos' ).value !== null && this.addressForm.get( 'requiereTransladoRecursos' ).value === false ) {
        this.addressForm.get( 'justificacionTrasladoAportanteFuente' ).setValue( '' );
    }

    if ( this.balanceFinanciero !== undefined ) {
      this.balanceFinanciero.requiereTransladoRecursos = this.addressForm.get( 'requiereTransladoRecursos' ).value;
      this.balanceFinanciero.justificacionTrasladoAportanteFuente = this.addressForm.get( 'justificacionTrasladoAportanteFuente' ).value;
      this.balanceFinanciero.urlSoporte = this.addressForm.get( 'urlSoporte' ).value
    }

    const pBalanceFinanciero = {
        balanceFinancieroId: this.addressForm.get( 'balanceFinancieroId' ).value,
        proyectoId:this.id,
        requiereTransladoRecursos: this.addressForm.get( 'requiereTransladoRecursos' ).value,
        justificacionTrasladoAportanteFuente: this.addressForm.get( 'justificacionTrasladoAportanteFuente' ).value,
        urlSoporte: this.addressForm.get( 'urlSoporte' ).value
    };

    this.financialBalanceService.createEditBalanceFinanciero( this.balanceFinanciero !== undefined ? this.balanceFinanciero : pBalanceFinanciero )
    .subscribe((respuesta: Respuesta) => {
        this.openDialog('', respuesta.message);
        this.ngOnInit();
        return; 
      },
      err => {
        this.openDialog('', err.message);
        this.ngOnInit();
        return;
      });
  }
}
