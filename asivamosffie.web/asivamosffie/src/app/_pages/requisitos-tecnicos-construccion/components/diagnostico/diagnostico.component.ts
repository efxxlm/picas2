import { Component, ElementRef, OnInit, Output, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-diagnostico',
  templateUrl: './diagnostico.component.html',
  styleUrls: ['./diagnostico.component.scss']
})
export class DiagnosticoComponent implements OnInit {

  booleanCheckbox: boolean;
  formDiagnostico: FormGroup;
  @ViewChild( 'valorTotalFaseConstruccion', { static: true } ) totalFaseConstruccion: ElementRef;

  solicitudesModificacion: any[] = [
    {value: 'PI_00089', viewValue: 'PI_00089'}
  ]

  constructor ( private activatedRoute: ActivatedRoute,
                private fb: FormBuilder,
                private dialog: MatDialog ) {
    console.log( this.activatedRoute.snapshot.params.id );
    this.crearFormulario();
    this.valorTotalFaseConstruccion();
  }

  ngOnInit(): void {
  };

  crearFormulario () {
    this.formDiagnostico = this.fb.group({
      booleanCheckbox: [ null ],
      urlSoporte: [ '' ],
      costoDirecto: [ '' ],
      administracion: [ '' ],
      imprevistos: [ '' ],
      utilidad: [ '' ],
      valorTotalFase2Construccion: [ '' ],
      modificacionContractual: [ null ],
      numeroSolicitudModificacion: [ null ]
    });
  };

  valorTotalFaseConstruccion () {
    this.formDiagnostico.valueChanges.subscribe( ( values: any ) => {
      this.totalFaseConstruccion.nativeElement.value = Number( values.costoDirecto )+Number( values.administracion )+Number( values.imprevistos )+Number( values.utilidad );
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
    console.log( this.totalFaseConstruccion.nativeElement.value );
    this.openDialog( 'La información ha sido guardada exitosamente', '' );
  };

};