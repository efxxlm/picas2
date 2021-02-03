import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { FaseDosAprobarConstruccionService } from 'src/app/core/_services/faseDosAprobarConstruccion/fase-dos-aprobar-construccion.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { TiposObservacionConstruccion } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';

@Component({
  selector: 'app-diagnostico-artc',
  templateUrl: './diagnostico-artc.component.html',
  styleUrls: ['./diagnostico-artc.component.scss']
})
export class DiagnosticoArtcComponent implements OnInit {

  addressForm = this.fb.group({
    tieneObservaciones: [null, Validators.required],
    observaciones: [null],
    construccionObservacionId: []
  });
  editorStyle = {
    height: '100px'
  };
  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };
  @Input() observacionesCompleted: boolean;
  @Input() construccion: any;
  @Input() contratoConstruccionId: any;
  @Output() createEditDiagnostico = new EventEmitter();
  @Output() estadoSemaforoDiagnostico = new EventEmitter<string>();
  totalGuardados = 0;
  observacionDiagnostico = '1';
  dataTablaHistorialObservacion: any[] = [];
  dataTableHistorialApoyo: any[] = [];
  dataSource = new MatTableDataSource();
  dataSourceApoyo = new MatTableDataSource();
  displayedColumns: string[] = [
    'fechaRevision',
    'observacionesSupervision'
  ];
  estaEditando = false;

  constructor(
    private dialog: MatDialog,
    private fb: FormBuilder,
    private faseDosAprobarConstruccionSvc: FaseDosAprobarConstruccionService
  )
  {
  }

  ngOnInit(): void {
    if (this.construccion) {
      this.addressForm.get('tieneObservaciones')
        .setValue(
          this.construccion.tieneObservacionesDiagnosticoSupervisor !== undefined ?
          this.construccion.tieneObservacionesDiagnosticoSupervisor : null
        );
      this.addressForm.get('observaciones')
        .setValue(
          this.construccion.observacionDiagnosticoSupervisor !== undefined ?
          this.construccion.observacionDiagnosticoSupervisor.observaciones : null
        );
      this.addressForm.get('construccionObservacionId')
        .setValue(
          this.construccion.observacionDiagnosticoSupervisor !== undefined ?
          this.construccion.observacionDiagnosticoSupervisor.construccionObservacionId : null
        );
      this.getDataTable();
    }
  }

  getDataTable() {
    this.construccion.construccionObservacion.forEach( observacion => {
      if (  observacion.tipoObservacionConstruccion === this.observacionDiagnostico
            && observacion.observaciones !== undefined
            && observacion.esSupervision === true ) {
        this.dataTablaHistorialObservacion.push( observacion );
      }
      if (  observacion.tipoObservacionConstruccion === this.observacionDiagnostico
        && observacion.observaciones !== undefined
        && observacion.esSupervision === false ) {
    this.dataTableHistorialApoyo.push( observacion );
  }
    } );
    if ( this.dataTablaHistorialObservacion.length > 0 ) {
      this.dataSource = new MatTableDataSource( this.dataTablaHistorialObservacion );
    }
    if ( this.dataTableHistorialApoyo.length > 0 ) {
      this.dataSourceApoyo = new MatTableDataSource( this.dataTableHistorialApoyo );
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
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data : { modalTitle, modalText }
    });
  }

  guardarDiagnostico() {
    this.estaEditando = true;
    const construccion = {
      contratoConstruccionId: this.contratoConstruccionId,
      tieneObservacionesDiagnosticoSupervisor: this.addressForm.value.tieneObservaciones,
      construccionObservacion: [
        {
          construccionObservacionId: this.addressForm.value.construccionObservacionId,
          contratoConstruccionId: this.contratoConstruccionId,
          tipoObservacionConstruccion: TiposObservacionConstruccion.Diagnostico,
          esSupervision: true,
          esActa: false,
          observaciones: this.addressForm.value.observaciones
        }
      ]
    };

    // console.log(construccion);

    if (  this.addressForm.value.tieneObservaciones === false
          && this.totalGuardados === 0
          && this.construccion.tieneObservacionesDiagnosticoApoyo === true ) {
      this.openDialog( '', '<b>Le recomendamos verificar su respuesta; tenga en cuenta que el apoyo a la supervisión si tuvo observaciones.</b>' );
      this.totalGuardados++;
      // console.log( 'condicion 1' );
      return;
    }
    if ( this.totalGuardados === 1 || this.addressForm.value.tieneObservaciones !== null ) {
      // console.log( 'condicion 2' );
      this.faseDosAprobarConstruccionSvc.createEditObservacionDiagnosticoSupervisor( construccion )
        .subscribe(
          response => {
            this.openDialog( '', response.message );
            this.createEditDiagnostico.emit( true );
          },
          err => this.openDialog( '', err.message )
        );
    }

  }

}
