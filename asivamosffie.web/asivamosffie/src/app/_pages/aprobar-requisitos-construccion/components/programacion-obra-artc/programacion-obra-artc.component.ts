import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { FaseDosAprobarConstruccionService } from 'src/app/core/_services/faseDosAprobarConstruccion/fase-dos-aprobar-construccion.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { TiposObservacionConstruccion } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';

@Component({
  selector: 'app-programacion-obra-artc',
  templateUrl: './programacion-obra-artc.component.html',
  styleUrls: ['./programacion-obra-artc.component.scss']
})
export class ProgramacionObraArtcComponent implements OnInit {

  addressForm = this.fb.group({
    tieneObservaciones: [null, Validators.required],
    observaciones: [null, Validators.required],
    construccionObservacionId: [],
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
  observacionProgramacionObra = '5';
  totalGuardados = 0;
  dataTablaHistorialObservacion: any[] = [];
  dataTablaHistorialApoyo: any[] = [];
  dataSource = new MatTableDataSource();
  dataSourceApoyo = new MatTableDataSource();
  displayedColumns: string[] = [
    'fechaCreacion',
    'observaciones'
  ];
  @Input() observacionesCompleted;
  @Input() contratoConstruccion: any;
  @Input() contratoConstruccionId: any;
  @Output() createEdit = new EventEmitter();
  estaEditando = false;

  constructor(
    private dialog: MatDialog,
    private fb: FormBuilder,
    private commonSvc: CommonService,
    private faseDosAprobarConstruccionSvc: FaseDosAprobarConstruccionService
  )
  { }

  ngOnInit(): void {
    this.addressForm.get('tieneObservaciones')
      .setValue(
        this.contratoConstruccion.tieneObservacionesProgramacionObraSupervisor !== undefined ?
        this.contratoConstruccion.tieneObservacionesProgramacionObraSupervisor : null
      );
    this.addressForm.get('observaciones')
      .setValue(
        this.contratoConstruccion.observacionProgramacionObraSupervisor !== undefined ?
        this.contratoConstruccion.observacionProgramacionObraSupervisor.observaciones : null );
    this.addressForm.get('construccionObservacionId')
      .setValue(
        this.contratoConstruccion.observacionProgramacionObraSupervisor !== undefined ?
        this.contratoConstruccion.observacionProgramacionObraSupervisor.construccionObservacionId : null );

    this.estaEditando = true;
    this.addressForm.markAllAsTouched();
    this.getDataTable();
  }

  getDataTable() {
    this.contratoConstruccion.construccionObservacion.forEach( observacion => {
      if (  observacion.tipoObservacionConstruccion === this.observacionProgramacionObra
            && observacion.observaciones !== undefined
            && observacion.esSupervision === true ) {
        this.dataTablaHistorialObservacion.push( observacion );
      }
      if (  observacion.tipoObservacionConstruccion === this.observacionProgramacionObra
            && observacion.observaciones !== undefined
            && observacion.esSupervision === false ) {
        this.dataTablaHistorialApoyo.push( observacion );
      }
    } );
    if ( this.dataTablaHistorialObservacion.length > 0 ) {
      this.dataSource = new MatTableDataSource( this.dataTablaHistorialObservacion );
    }
    if ( this.dataTablaHistorialApoyo.length > 0 ) {
      this.dataSourceApoyo = new MatTableDataSource( this.dataTablaHistorialApoyo );
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
      data: { modalTitle, modalText }
    });
  }

  descargar() {
    this.commonSvc.getFileById(this.contratoConstruccion.archivoCargueIdProgramacionObra)
      .subscribe(respuesta => {
        const documento = 'ProgramacionObra.xlsx';
        const blob = new Blob([respuesta], { type: 'application/octet-stream' });
        const anchor = document.createElement('a');
        anchor.download = documento;
        anchor.href = window.URL.createObjectURL(blob);
        anchor.dataset.downloadurl = ['application/octet-stream', anchor.download, anchor.href].join(':');
        anchor.click();
      });
  }

  guardarProgramacion() {
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();
    const construccion = {
      contratoConstruccionId: this.contratoConstruccionId,
      tieneObservacionesProgramacionObraSupervisor: this.addressForm.value.tieneObservaciones,
      construccionObservacion: [
        {
          construccionObservacionId: this.addressForm.value.construccionObservacionId,
          contratoConstruccionId: this.contratoConstruccionId,
          tipoObservacionConstruccion: TiposObservacionConstruccion.ProgramacionObra,
          esSupervision: true,
          esActa: false,
          observaciones: this.addressForm.value.observaciones
        }
      ]
    };

    // console.log( construccion );

    if (  this.addressForm.value.tieneObservaciones === false
          && this.totalGuardados === 0
          && this.contratoConstruccion.tieneObservacionesProgramacionObraApoyo === true ) {
      this.openDialog( '', '<b>Le recomendamos verificar su respuesta; tenga en cuenta que el apoyo a la supervisi??n si tuvo observaciones.</b>' );
      this.totalGuardados++;
      return;
    }
    if ( this.totalGuardados === 1 || this.addressForm.value.tieneObservaciones !== null ) {
      this.faseDosAprobarConstruccionSvc.createEditObservacionProgramacionObraSupervisor( construccion )
        .subscribe(
          response => {
            this.openDialog( '', response.message );
            this.createEdit.emit( true );
          },
          err => this.openDialog( '', err.message )
        );
    }

  }

}
