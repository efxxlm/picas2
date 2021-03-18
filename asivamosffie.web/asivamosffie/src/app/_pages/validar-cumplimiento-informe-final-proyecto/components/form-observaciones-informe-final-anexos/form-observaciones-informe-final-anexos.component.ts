import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ValidarCumplimientoInformeFinalService } from 'src/app/core/_services/validarCumplimientoInformeFinal/validar-cumplimiento-informe-final.service';
import { Report } from 'src/app/_interfaces/proyecto-final.model';
import { Respuesta } from 'src/app/core/_services/common/common.service';
import { InformeFinalInterventoria } from 'src/app/_interfaces/proyecto-final-anexos.model';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';

@Component({
  selector: 'app-form-observaciones-informe-final-anexos',
  templateUrl: './form-observaciones-informe-final-anexos.component.html',
  styleUrls: ['./form-observaciones-informe-final-anexos.component.scss']
})
export class FormObservacionesInformeFinalAnexosComponent implements OnInit {

  @Input() report: Report;
  ELEMENT_DATA : InformeFinalInterventoria[] = [];
  estaEditando = false;
  informeFinalId = 0;
  tieneObservacionesInterventoria = null;
  observaciones: FormGroup;
  noGuardado=true; 
  existe_historial = false;
  dataSource = new MatTableDataSource<InformeFinalInterventoria>(this.ELEMENT_DATA);
  anexos: any;

  //observaciones supervisor
  observacionesForm = this.fb.group({
    observaciones: [null, Validators.required],
    fechaCreacion: [null, Validators.required],   
  })

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
    private dialog: MatDialog, private fb: FormBuilder,
    private validarCumplimientoInformeFinalService: ValidarCumplimientoInformeFinalService,
    private router: Router,
    ) {}

  ngOnInit(): void {
    this.buildForm();
  }

  private buildForm() {
    this.observaciones = this.fb.group({ 
      informeFinalObservacionesId: [null, Validators.required],
      informeFinalId: [null, Validators.required],
      observaciones: [null, Validators.required],
      eliminado: [null, Validators.required],
      archivado: [null, Validators.required],
      tieneObservacionesInterventoria: [null, Validators.required],
      esGrupoNovedadesInterventoria: [null, Validators.required],
    });
    console.log(this.report);
    if(this.report){
      if (this.report.proyecto.informeFinal.length > 0) {
        this.observaciones.get("informeFinalId").setValue(this.report.proyecto.informeFinal[0].informeFinalId);
        this.observaciones.get("esGrupoNovedadesInterventoria").setValue(true);
        if(this.report.proyecto.informeFinal[0].tieneObservacionesInterventoria != null){
          this.observaciones.get("tieneObservacionesInterventoria").setValue(this.report.proyecto.informeFinal[0].tieneObservacionesInterventoria);
        }
        if(this.report.proyecto.informeFinal[0].informeFinalObservacionesInterventoria.length>0){
          this.observaciones.patchValue(this.report.proyecto.informeFinal[0].informeFinalObservacionesInterventoria[0]);
        }
        if(this.report.proyecto.informeFinal[0].historialObsInformeFinalInterventoriaNovedades.length>0){
          this.existe_historial = true;
        }
        this.estaEditando = true;
      }
    }
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  openDialogSuccess(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText },
    });
  
    dialogRef.afterClosed().subscribe(result => {
      this.router.navigate(['/validarCumplimientoInformeFinalProyecto']);
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

  onSubmit(test: boolean) {
    this.noGuardado = false;
    console.log(this.observaciones.value);
    this.estaEditando = true;
    this.observaciones.markAllAsTouched();
    this.estaEditando = true;
    this.createEditObservacionInformeFinalInterventoria(this.observaciones.value, this.observaciones.value.tieneObservacionesInterventoria, test);
  }

  
  createEditObservacionInformeFinalInterventoria(informeFinalObservacion: any, tieneObservaciones: boolean, test: boolean) {
    this.validarCumplimientoInformeFinalService.createEditObservacionInformeFinalInterventoria(informeFinalObservacion, tieneObservaciones).subscribe((respuesta: Respuesta) => {
      if(!test){
        this.openDialogSuccess('', respuesta.message);
      }else{
        this.openDialog('', respuesta.message);
      }
    });
  }

    /*getObservacionesByInformeFinalInterventoriaId(id: string) {
      this.validarCumplimientoInformeFinalService.getObservacionesByInformeFinalInterventoriaId(id).subscribe(anexos => {
          this.dataSource.data = anexos as InformeFinalInterventoria[];
          this.anexos = anexos;
          if(this.anexos.observacionVigenteSupervisor != null){
            this.observacionesForm.patchValue(this.anexos.observacionVigenteSupervisor)
          }
          if(this.anexos.historialInformeFinalInterventoriaObservaciones != null){
            if(this.anexos.historialInformeFinalInterventoriaObservaciones.length > 0){
              if(this.anexos.observacionVigenteSupervisor != null){
                this.existe_historial = true;
              }
            }      
          }
      });
    }*/
}
