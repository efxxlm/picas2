import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { RegisterContractualLiquidationRequestService } from 'src/app/core/_services/registerContractualLiquidationRequest/register-contractual-liquidation-request.service';

@Component({
  selector: 'app-form-observacion-actualizacion-poliza',
  templateUrl: './form-observacion-actualizacion-poliza.component.html',
  styleUrls: ['./form-observacion-actualizacion-poliza.component.scss']
})
export class FormObservacionActualizacionPolizaComponent implements OnInit {

  
  @Input() contratacionId: any;
  @Input() tipoObservacionCodigo: string;
  @Input() menuId: any;
  @Input() contratoPolizaActualizacionId: number;
  @Input() esVerDetalle: boolean;
  @Output("callOnInitParent") callOnInitParent: EventEmitter<any> = new EventEmitter();


  observaciones: FormGroup = this.fb.group({
    liquidacionContratacionObservacionId: [null, Validators.required],
    tieneObservacion: [null, Validators.required],
    observacion: [null, Validators.required],
    fechaCreacion: [null, Validators.required]
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
  estaEditando = false;

  constructor(
    private dialog: MatDialog,
    private fb: FormBuilder,
    private registerContractualLiquidationRequestService: RegisterContractualLiquidationRequestService
  ) { }

  ngOnInit(): void {
    this.buildForm();
  }

  buildForm() {
    console.log(this.menuId,this.contratacionId,this.contratoPolizaActualizacionId,this.tipoObservacionCodigo);
    this.registerContractualLiquidationRequestService.getObservacionLiquidacionContratacionByMenuIdAndContratacionId(
      this.menuId,
      this.contratacionId,
      this.contratoPolizaActualizacionId,
      this.tipoObservacionCodigo
    ).subscribe(response => {
      if(response != null && response.length > 0){
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
    dialogRef.afterClosed().subscribe(result => {
      this.callOnInitParent.emit();
      return;
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
        contratacionId: this.contratacionId,
        tipoObservacionCodigo: this.tipoObservacionCodigo,
        tieneObservacion: this.observaciones.get( 'tieneObservacion' ).value !== null ? this.observaciones.get( 'tieneObservacion' ).value : this.observaciones.get( 'tieneObservacion' ).value,
        observacion: this.observaciones.get( 'observacion' ).value !== null ? this.observaciones.get( 'observacion' ).value : this.observaciones.get( 'observacion' ).value,
        menuId: this.menuId,
        idPadre: this.contratoPolizaActualizacionId
      };

    this.registerContractualLiquidationRequestService.createUpdateLiquidacionContratacionObservacion( pLiquidacionContratacionObservacion )
        .subscribe(
            response => {
                this.openDialog( '', `<b>${ response.message }</b>` );
                return;
            },
            err => this.openDialog( '', `<b>${ err.message }</b>` )
        )
  }

}
