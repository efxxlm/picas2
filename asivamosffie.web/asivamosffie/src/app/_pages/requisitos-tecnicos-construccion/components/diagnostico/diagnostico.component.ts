import { Component, ElementRef, Input, OnInit, Output, ViewChild, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-diagnostico',
  templateUrl: './diagnostico.component.html',
  styleUrls: ['./diagnostico.component.scss']
})
export class DiagnosticoComponent implements OnInit {

  booleanCheckbox: boolean;
  formDiagnostico: FormGroup;
  totalConstruccion: number;
  @Input() contratoConstruccion: any[] = [];
  @Output() diagnostico = new EventEmitter();
  @ViewChild( 'valorTotalFaseConstruccion', { static: true } ) totalFaseConstruccion: ElementRef;

  solicitudesModificacion: any[] = [
    {value: 'PI_00089', viewValue: 'PI_00089'}
  ]

  constructor ( private fb: FormBuilder,
                private dialog: MatDialog,
                private currencyPipe: CurrencyPipe ) 
  {
    this.crearFormulario();
    this.valorTotalFaseConstruccion();
  }

  ngOnInit(): void {
  };

  crearFormulario () {
    this.formDiagnostico = this.fb.group({
      esInformeDiagnostico: [ null ],
      rutaInforme: [ '' ],
      costoDirecto: [ null ],
      administracion: [ null ],
      imprevistos: [ null ],
      utilidad: [ null ],
      valorTotalFaseConstruccion: [ null ],
      requiereModificacionContractual: [ null ],
      numeroSolicitudModificacion: [ null ]
    });
  };

  valorTotalFaseConstruccion () {
    this.formDiagnostico.valueChanges.subscribe( ( values: any ) => {
      const totalFase = Number( values.costoDirecto )+Number( values.administracion )+Number( values.imprevistos )+Number( values.utilidad );
      this.totalConstruccion = totalFase;
      this.totalFaseConstruccion.nativeElement.value = this.currencyPipe.transform( totalFase, 'COP', 'symbol-narrow', '.0-0' );
    } );
  };

  openDialog (modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data : { modalTitle, modalText }
    });
  };

  enviar () {
    console.log( this.formDiagnostico );
    console.log( this.totalConstruccion );
    this.formDiagnostico.get( 'valorTotalFaseConstruccion' ).setValue( this.totalConstruccion );
    this.diagnostico.emit( this.formDiagnostico.value );
  };

};