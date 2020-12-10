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
  @Input() contratoConstruccion;
  @Output() diagnostico = new EventEmitter();
  @ViewChild( 'valorTotalFaseConstruccion', { static: true } ) totalFaseConstruccion: ElementRef;

  estaEditando = false;


  solicitudesModificacion: any[] = [

  ];

  constructor( private fb: FormBuilder,
               private dialog: MatDialog,
               private currencyPipe: CurrencyPipe )
  {
    this.crearFormulario();
    this.valorTotalFaseConstruccion();
  }

  ngOnInit(): void {
    if ( this.contratoConstruccion ) {
      this.formDiagnostico.setValue(
        {
          esInformeDiagnostico           : this.contratoConstruccion.esInformeDiagnostico !== undefined ? this.contratoConstruccion.esInformeDiagnostico : null,
          rutaInforme                    : this.contratoConstruccion.rutaInforme !== undefined ? this.contratoConstruccion.rutaInforme : null,
          costoDirecto                   : this.contratoConstruccion.costoDirecto !== undefined ? this.contratoConstruccion.costoDirecto : null,
          administracion                 : this.contratoConstruccion.administracion !== undefined ? this.contratoConstruccion.administracion : null,
          imprevistos                    : this.contratoConstruccion.imprevistos !== undefined ? this.contratoConstruccion.imprevistos : null,
          utilidad                       : this.contratoConstruccion.utilidad !== undefined ? this.contratoConstruccion.utilidad : null,
          valorTotalFaseConstruccion     : this.contratoConstruccion.valorTotalFaseConstruccion ? this.contratoConstruccion.valorTotalFaseConstruccion : null,
          requiereModificacionContractual: this.contratoConstruccion.requiereModificacionContractual !== undefined ? this.contratoConstruccion.requiereModificacionContractual : null,
          numeroSolicitudModificacion    : this.contratoConstruccion.numeroSolicitudModificacion !== undefined ? this.contratoConstruccion.numeroSolicitudModificacion : null
        }
      );
      this.formDiagnostico.valueChanges.subscribe( ( values: any ) => {
        const totalFase = Number( values.costoDirecto ) + Number( values.administracion ) + Number( values.imprevistos ) + Number( values.utilidad );
        this.totalConstruccion = totalFase;
        this.totalFaseConstruccion.nativeElement.value = this.currencyPipe.transform( totalFase, 'COP', 'symbol-narrow', '.0-0' );
      } );
    }
  }

  crearFormulario() {
    this.formDiagnostico = this.fb.group({
      esInformeDiagnostico: [null, Validators.required],
      rutaInforme: [null, Validators.required],
      costoDirecto: [null, Validators.required],
      administracion: [null, Validators.required],
      imprevistos: [null, Validators.required],
      utilidad: [null, Validators.required],
      valorTotalFaseConstruccion: [null, Validators.required],
      requiereModificacionContractual: [null, Validators.required],
      numeroSolicitudModificacion: [null, Validators.required]
    });
  }

  valorTotalFaseConstruccion() {
    this.formDiagnostico.valueChanges.subscribe( ( values: any ) => {
      const totalFase = Number( values.costoDirecto ) + Number( values.administracion ) + Number( values.imprevistos ) + Number( values.utilidad );
      this.totalConstruccion = totalFase;
      this.totalFaseConstruccion.nativeElement.value = this.currencyPipe.transform( totalFase, 'COP', 'symbol-narrow', '.0-0' );
      this.estaEditando = true;
    } );
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data : { modalTitle, modalText }
    });
  }

  enviar() {

    this.formDiagnostico.get( 'valorTotalFaseConstruccion' ).setValue( this.totalConstruccion );
    console.log( this.formDiagnostico.value.requiereModificacionContractual );
    const diagnostico = {
      esInformeDiagnostico: this.formDiagnostico.get( 'esInformeDiagnostico' ).value,
      rutaInforme: this.formDiagnostico.get( 'rutaInforme' ).value,
      costoDirecto: this.formDiagnostico.get( 'costoDirecto' ).value,
      administracion: this.formDiagnostico.get( 'administracion' ).value,
      imprevistos: this.formDiagnostico.get( 'imprevistos' ).value,
      utilidad: this.formDiagnostico.get( 'utilidad' ).value,
      valorTotalFaseConstruccion: this.formDiagnostico.get( 'valorTotalFaseConstruccion' ).value,
      requiereModificacionContractual: this.formDiagnostico.get( 'requiereModificacionContractual' ).value,
      numeroSolicitudModificacion: this.formDiagnostico.get( 'numeroSolicitudModificacion' ).value,
    };
    this.diagnostico.emit( diagnostico );
  }

}
