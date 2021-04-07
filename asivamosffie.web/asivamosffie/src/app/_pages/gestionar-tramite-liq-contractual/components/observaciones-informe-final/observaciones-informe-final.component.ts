import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { RegisterContractualLiquidationRequestService } from 'src/app/core/_services/registerContractualLiquidationRequest/register-contractual-liquidation-request.service';


@Component({
  selector: 'app-observaciones-informe-final',
  templateUrl: './observaciones-informe-final.component.html',
  styleUrls: ['./observaciones-informe-final.component.scss']
})
export class ObservacionesInformeFinalComponent implements OnInit {

  estaEditando = false;
  @Input() contratacionProyectoId: any;
  @Input() tipoObservacionCodigo: string;
  @Input() menuId: any;
  @Input() informeFinalId: number;

  observaciones: FormGroup = this.fb.group({
    liquidacionContratacionObservacionId: [null, Validators.required],
    tieneObservacion: [null, Validators.required],
    observacion: [null, Validators.required]
  });


  editorStyle = {
    height: '100px'
  };
  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }]
    ]
  };

  constructor(
    private dialog: MatDialog,
    private fb: FormBuilder,
    private registerContractualLiquidationRequestService: RegisterContractualLiquidationRequestService
  ) { }

  ngOnInit(): void {
    this.buildForm();
  }

  buildForm() {
    this.registerContractualLiquidationRequestService.getObservacionLiquidacionContratacionByMenuIdAndContratacionProyectoId(
      this.menuId,
      this.contratacionProyectoId,
      this.informeFinalId,
      this.tipoObservacionCodigo
    ).subscribe(response => {
      if(response != null){
        this.observaciones.patchValue(response[0]);
      }
    });
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n - 1, e.editor.getLength());
    }
  }

  textoLimpio(evento: any, n: number) {
    if (evento !== undefined) {
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
    this.observaciones.markAllAsTouched();
    if ( this.observaciones.get( 'tieneObservacion' ).value !== null && this.observaciones.get( 'tieneObservacion' ).value === false ) {
        this.observaciones.get( 'observacion' ).setValue( '' );
    }

    const pLiquidacionContratacionObservacion = {
        liquidacionContratacionObservacionId: this.observaciones.get( 'liquidacionContratacionObservacionId' ).value,
        contratacionProyectoId: this.contratacionProyectoId,
        tipoObservacionCodigo: this.tipoObservacionCodigo,
        tieneObservacion: this.observaciones.get( 'tieneObservacion' ).value !== null ? this.observaciones.get( 'tieneObservacion' ).value : this.observaciones.get( 'tieneObservacion' ).value,
        observacion: this.observaciones.get( 'observacion' ).value !== null ? this.observaciones.get( 'observacion' ).value : this.observaciones.get( 'observacion' ).value,
        menuId: this.menuId,
        idPadre: this.informeFinalId
      };

    this.registerContractualLiquidationRequestService.createUpdateLiquidacionContratacionObservacion( pLiquidacionContratacionObservacion )
        .subscribe(
            response => {
                this.openDialog( '', `<b>${ response.message }</b>` );
                this.ngOnInit();
                return;
            },
            err => this.openDialog( '', `<b>${ err.message }</b>` )
        )
  }
}
