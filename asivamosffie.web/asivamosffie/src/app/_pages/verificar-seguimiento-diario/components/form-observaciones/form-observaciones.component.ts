import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { FollowUpDailyService } from 'src/app/core/_services/dailyFollowUp/daily-follow-up.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { SeguimientoDiario, SeguimientoDiarioObservaciones } from 'src/app/_interfaces/DailyFollowUp';

@Component({
  selector: 'app-form-observaciones',
  templateUrl: './form-observaciones.component.html',
  styleUrls: ['./form-observaciones.component.scss']
})
export class FormObservacionesComponent implements OnInit, OnChanges {

  addressForm = this.fb.group({
    tieneObservaciones: [null, Validators.required],
    observacion: [null, Validators.required]
  });

  minDate: Date;
  editorStyle = {
    height: '45px'
  };
  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  seguimientoId?: number;
  @Input() observacionObjeto: SeguimientoDiarioObservaciones;
  @Input() tieneObservaciones?: boolean;

  constructor(
    private fb: FormBuilder,
    private dialog: MatDialog,
    private followUpDailyService: FollowUpDailyService,
    private route: ActivatedRoute,
    private router: Router,

  ) 
  { 
    
  }

  ngOnChanges(changes: SimpleChanges): void {
    if ( changes.observacionObjeto ){
      this.addressForm.get('observacion').setValue( this.observacionObjeto ? this.observacionObjeto.observaciones : null );
    }
    if ( changes.tieneObservaciones ){
      this.addressForm.get('tieneObservaciones').setValue( this.tieneObservaciones );
    }
    console.log('t',changes)

  }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.seguimientoId = params.id;
      console.log(this.seguimientoId, this.observacionObjeto, this.tieneObservaciones);

      this.addressForm.get('observacion').setValue( this.observacionObjeto ? this.observacionObjeto.observaciones : null );
      this.addressForm.get('tieneObservaciones').setValue( this.tieneObservaciones );

      
    });
  }

  textoLimpio(texto: string) {
    if (texto) {
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
    }
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {
    this.addressForm.markAllAsTouched();

    let seguimiento: SeguimientoDiario = {
      seguimientoDiarioId: this.seguimientoId,
      tieneObservacionApoyo: this.addressForm.value.tieneObservaciones,

      seguimientoDiarioObservaciones: [
        {
          seguimientoDiarioObservacionesId: this.observacionObjeto ? this.observacionObjeto.seguimientoDiarioObservacionesId : 0,
          seguimientoDiarioId: this.seguimientoId,
          esSupervision: false,
          observaciones: this.addressForm.value.observacion
        }
      ]
    }

    this.followUpDailyService.createEditObservacion( seguimiento, false )
      .subscribe( respuesta => {
        this.openDialog('', respuesta.message);
        if ( respuesta.code == "200" ){
          this.router.navigate(['/verificarSeguimientoDiario']);
        }


      });
  }
}
