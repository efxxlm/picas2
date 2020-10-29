import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { CompromisosActasComiteService } from '../../../../core/_services/compromisosActasComite/compromisos-actas-comite.service';

@Component({
  selector: 'app-reporte-avance-compromiso',
  templateUrl: './reporte-avance-compromiso.component.html',
  styleUrls: ['./reporte-avance-compromiso.component.scss']
})
export class ReporteAvanceCompromisoComponent implements OnInit, OnDestroy {

  reporte: FormGroup;
  estadoBoolean = false;
  estadoComite = '';
  comite: any = {};
  estadoCodigo: string;
  seRealizoPeticion = false;
  estados: any[] = [
    { value: '1', viewValue: 'Sin iniciar' },
    { value: '2', viewValue: 'En proceso' },
    { value: '3', viewValue: 'Finalizada' }
  ];
  editorStyle = {
    height: '60px'
  };
  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  constructor(private routes: Router,
              public dialog: MatDialog,
              private fb: FormBuilder,
              private activatedRoute: ActivatedRoute,
              private compromisoSvc: CompromisosActasComiteService) {
    this.getComite();
    this.crearFormulario();
  }

  async ngOnInit() {
    if (this.comite) {
      const observacion = await this.compromisoSvc.cargarObservacionGestion(this.comite.compromisoId);
      if (this.comite.estadoCodigo === '2' || this.comite.estadoCodigo === '3') {
        this.estadoCodigo = this.comite.estadoCodigo;
      }
      if (observacion !== null) {
        this.reporte.get('reporteEstado').setValue(observacion);
      }
    }
  }

  ngOnDestroy(): void {
    if (this.estadoCodigo === undefined && this.reporte.get('reporteEstado').value !== null) {
      if (this.seRealizoPeticion === false && this.comite.estadoCodigo !== '3') {
        this.openDialogConfirmar('', '¿Desea guardar la información registrada?');
      }
    }
    if (this.estadoCodigo !== undefined && this.reporte.get('reporteEstado').value !== null) {
      if (this.seRealizoPeticion === false && this.comite.estadoCodigo !== '3') {
        this.openDialogConfirmar('', '¿Desea guardar la información registrada?');
      }
    }
  }

  openDialogConfirmar(modalTitle: string, modalText: string) {
    const confirmarDialog = this.dialog.open(ModalDialogComponent, {
      width: '30em',
      data: { modalTitle, modalText, siNoBoton: true }
    });

    confirmarDialog.afterClosed()
      .subscribe(response => {
        if (response === true) {
          if (this.estadoCodigo === undefined && this.reporte.get('reporteEstado').value !== null) {
            this.openDialog('', 'Debe seleccionar el estado del Compromiso');
          }
          this.compromisoSvc.guardarObservacionStorage(this.reporte.get('reporteEstado').value, this.comite.compromisoId);
        }
      });
  }

  getComite() {
    if (this.routes.getCurrentNavigation().extras.replaceUrl || undefined) {
      this.routes.navigate(['/compromisosActasComite']);
      return;
    }
    this.comite = this.routes.getCurrentNavigation().extras.state.elemento;
    console.log(this.comite);
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    if (texto) {
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
    }
  }

  textoLimpioMessage(texto: string) {
    if (texto) {
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio;
    }
  }

  crearFormulario() {
    this.reporte = this.fb.group({
      reporteEstado: [null, Validators.required]
    });
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {

    if (this.reporte.invalid || this.estadoCodigo === undefined) {
      this.openDialog('', '<b>Falta registrar información</b>');
      return;
    }

    this.comite.tarea = this.reporte.get('reporteEstado').value;

    this.compromisoSvc.postCompromisos(this.comite, this.estadoCodigo)
      .subscribe(
        resp => {
          this.seRealizoPeticion = true;
          this.compromisoSvc.eliminarObservacionStorage(this.comite.compromisoId);
          this.openDialog('', this.textoLimpioMessage(resp.message));
          this.routes.navigate(['/compromisosActasComite']);
        },
        err => this.openDialog('', err.message)
      );

  }

}
