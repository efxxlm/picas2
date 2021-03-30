import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { Respuesta } from 'src/app/core/_services/common/common.service';
import { RegisterProjectEtcService } from 'src/app/core/_services/registerProjectETC/register-project-etc.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { RepresentanteETCRecorrido } from 'src/app/_interfaces/proyecto-entrega-etc';

@Component({
  selector: 'app-form-representante',
  templateUrl: './form-representante.component.html',
  styleUrls: ['./form-representante.component.scss']
})
export class FormRepresentanteComponent implements OnInit {
  @Input() numRepresentantesRecorrido: any;
  @Input() proyectoEntregaEtcId: any;
  @Input() representanteEtcrecorrido: any;
  @Output('callOnSubmitParent') callOnSubmitParent: EventEmitter<any> = new EventEmitter();
  @Output('callOnInitParent') callOnInitParent: EventEmitter<any> = new EventEmitter();
  @Output() numRepresentantesRecorridoChange = new EventEmitter<string>();

  representantesForm: FormGroup;
  ELEMENT_DATA: RepresentanteETCRecorrido[] = [];
  dataSource = new MatTableDataSource<RepresentanteETCRecorrido>(this.ELEMENT_DATA);
  estaEditando = false;

  get representantes() {
    return this.representantesForm.get('representantes') as FormArray;
  }

  constructor(
    private fb: FormBuilder,
    public dialog: MatDialog,
    private registerProjectETCService: RegisterProjectEtcService
  ) {
    this.crearFormulario();
  }

  crearFormulario() {
    this.representantesForm = this.fb.group({
      numRepresentantesRecorrido: [''],
      representantes: this.fb.array([])
    });
  }

  ngOnInit(): void {
    if (this.numRepresentantesRecorrido != null) {
      if (this.representanteEtcrecorrido !== undefined && this.representanteEtcrecorrido.length > 0) {
        this.representantes.clear();
        for (const representante of this.representanteEtcrecorrido) {
          console.log(representante);
          this.representantes.push(
            this.fb.group({
              representanteEtcid: representante.representanteEtcid,
              proyectoEntregaEtcId: representante.proyectoEntregaEtcId,
              nombre: representante.nombre,
              cargo: representante.cargo,
              dependencia: representante.dependencia,
              semaforo:
                representante.registroCompleto == true
                  ? 'completo'
                  : (representante.registroCompleto == false &&
                      (representante.nombre == '' || representante.nombre == null) &&
                      (representante.cargo == '' || representante.cargo == null) &&
                      (representante.dependencia == '' || representante.dependencia == null)) ||
                    representante.registroCompleto == null
                  ? 'sin-diligenciar'
                  : 'en-proceso'
            })
          );
          if (this.estaEditando) this.representantesForm.markAllAsTouched();
        }
      } else {
        for (let i = 0; i < this.numRepresentantesRecorrido; i++) {
          this.representantes.push(
            this.fb.group({
              representanteEtcid: [null, Validators.required],
              proyectoEntregaEtcId: [this.proyectoEntregaEtcId, Validators.required],
              nombre: [null, Validators.required],
              cargo: [null, Validators.required],
              dependencia: [null, Validators.required],
              semaforo: 'sin-diligenciar'
            })
          );
          if (this.estaEditando) this.representantesForm.markAllAsTouched();
        }
      }
    }
  }

  ngOnChanges(changes) {
    if (
      changes.numRepresentantesRecorrido.currentValue != null &&
      changes.numRepresentantesRecorrido.currentValue != 'undefined'
    ) {
      this.representantesForm
        .get('numRepresentantesRecorrido')
        .setValue(this.numRepresentantesRecorrido, { emitEvent: true });
      this.representantesForm.markAllAsTouched();
      this.getCantidadRepresentantes();
      //primera vez
      if (!this.estaEditando) {
        this.ngOnInit();
      }
    }
  }

  getCantidadRepresentantes() {
    const value = this.representantesForm.get('numRepresentantesRecorrido').value;
    this.representantesForm.markAllAsTouched();
    this.estaEditando = true;
    if (this.representanteEtcrecorrido !== undefined && this.representanteEtcrecorrido.length > 0) {
      this.representantes.clear();
      for (const representante of this.representanteEtcrecorrido) {
        this.representantes.push(
          this.fb.group({
            representanteEtcid: representante.representanteEtcid,
            proyectoEntregaEtcId: representante.proyectoEntregaEtcId,
            nombre: representante.nombre,
            cargo: representante.cargo,
            dependencia: representante.dependencia,
            semaforo:
              representante.registroCompleto == true
                ? 'completo'
                : (representante.registroCompleto == false &&
                    (representante.nombre == '' || representante.nombre == null) &&
                    (representante.cargo == '' || representante.cargo == null) &&
                    (representante.dependencia == '' || representante.dependencia == null)) ||
                  representante.registroCompleto == null
                ? 'sin-diligenciar'
                : 'en-proceso'
          })
        );
        if (this.estaEditando) this.representantesForm.markAllAsTouched();
      }
      this.representantesForm
        .get('numRepresentantesRecorrido')
        .setValidators(Validators.min(this.representantes.length));
      const nuevosRepresentantes = Number(value) - this.representantes.length;
      if (Number(value) < this.representantes.length) {
        this.openDialog(
          '',
          '<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>'
        );
        this.representantesForm.get('numRepresentantesRecorrido').setValue(String(this.representantes.length));
        this.numRepresentantesRecorridoChange.emit(String(this.representantes.length));
        return;
      }
      for (let i = 0; i < nuevosRepresentantes; i++) {
        this.representantes.push(
          this.fb.group({
            representanteEtcid: [null, Validators.required],
            proyectoEntregaEtcId: [this.proyectoEntregaEtcId, Validators.required],
            nombre: [null, Validators.required],
            cargo: [null, Validators.required],
            dependencia: [null, Validators.required],
            semaforo: 'sin-diligenciar'
          })
        );
        if (this.estaEditando) this.representantesForm.markAllAsTouched();
      }
    } else if (this.representanteEtcrecorrido !== undefined && this.representanteEtcrecorrido.length === 0) {
      if (Number(value) < 0) {
        this.numRepresentantesRecorridoChange.emit('0');
        this.representantesForm.get('numRepresentantesRecorrido').setValue('0');
      }
      if (Number(value) > 0) {
        if (this.representantes.dirty === true) {
          this.numRepresentantesRecorridoChange.emit(String(this.representantes.length));
          this.representantesForm
            .get('numRepresentantesRecorrido')
            .setValidators(Validators.min(this.representantes.length));
          const nuevosRepresentantes = Number(value) - this.representantes.length;
          if (Number(value) < this.representantes.length && Number(value) > 0) {
            this.openDialog(
              '',
              '<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>'
            );
            this.numRepresentantesRecorridoChange.emit(String(this.representantes.length));
            this.representantesForm.get('numRepresentantesRecorrido').setValue(String(this.representantes.length));
            return;
          }
          for (let i = 0; i < nuevosRepresentantes; i++) {
            this.representantes.push(
              this.fb.group({
                representanteEtcid: [null, Validators.required],
                proyectoEntregaEtcId: [this.proyectoEntregaEtcId, Validators.required],
                nombre: [null, Validators.required],
                cargo: [null, Validators.required],
                dependencia: [null, Validators.required],
                semaforo: 'sin-diligenciar'
              })
            );
          if (this.estaEditando) this.representantesForm.markAllAsTouched();
        }
        } else {
          this.representantes.clear();
          for (let i = 0; i < Number(value); i++) {
            this.representantes.push(
              this.fb.group({
                representanteEtcid: [null, Validators.required],
                proyectoEntregaEtcId: [this.proyectoEntregaEtcId, Validators.required],
                nombre: [null, Validators.required],
                cargo: [null, Validators.required],
                dependencia: [null, Validators.required],
                semaforo: 'sin-diligenciar'
              })
            );
          if (this.estaEditando) this.representantesForm.markAllAsTouched();
        }
        }
      }
    } else if (this.representanteEtcrecorrido === undefined) {
      if (Number(value) < 0) {
        this.numRepresentantesRecorridoChange.emit('0');
        this.representantesForm.get('numRepresentantesRecorrido').setValue('0');
      }
      if (Number(value) > 0) {
        if (this.representantes.dirty === true) {
          this.representantesForm
            .get('numRepresentantesRecorrido')
            .setValidators(Validators.min(this.representantes.length));
          const nuevosRepresentantes = Number(value) - this.representantes.length;
          if (Number(value) < this.representantes.length && Number(value) > 0) {
            this.openDialog(
              '',
              '<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>'
            );
            this.numRepresentantesRecorridoChange.emit(String(this.representantes.length));
            this.representantesForm.get('numRepresentantesRecorrido').setValue(String(this.representantes.length));
            return;
          }
          for (let i = 0; i < nuevosRepresentantes; i++) {
            this.representantes.push(
              this.fb.group({
                representanteEtcid: [null, Validators.required],
                proyectoEntregaEtcId: [this.proyectoEntregaEtcId, Validators.required],
                nombre: [null, Validators.required],
                cargo: [null, Validators.required],
                dependencia: [null, Validators.required],
                semaforo: 'sin-diligenciar'
              })
            );
          if (this.estaEditando) this.representantesForm.markAllAsTouched();
        }
        } else {
          this.representantes.clear();
          for (let i = 0; i < Number(value); i++) {
            this.representantes.push(
              this.fb.group({
                representanteEtcid: [null, Validators.required],
                proyectoEntregaEtcId: [this.proyectoEntregaEtcId, Validators.required],
                nombre: [null, Validators.required],
                cargo: [null, Validators.required],
                dependencia: [null, Validators.required],
                semaforo: 'sin-diligenciar'
              })
            );
          if (this.estaEditando) this.representantesForm.markAllAsTouched();
        }
        }
      }
    }
  }

  openDialog(modalTitle: string, modalText: string) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  guardar() {
    this.estaEditando = true;
    this.representantesForm.markAllAsTouched();
    this.callOnSubmitParent.emit();
    return;
  }

  onSubmit() {
    this.estaEditando = true;
    this.representantesForm.markAllAsTouched();
    this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
  }

  openDialogTrueFalse(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton: true }
    });

    return dialogRef.afterClosed();
  }

  eliminarRepresentante(representanteEtcid: number, numeroRepresentante: number) {
    this.openDialogTrueFalse('', '¿Está seguro de eliminar esta información?').subscribe(value => {
      if (value === true) {
        if (representanteEtcid === 0 || representanteEtcid == null) {
          this.representantes.removeAt(numeroRepresentante);
          this.numRepresentantesRecorridoChange.emit(String(this.representantes.length));
          this.representantesForm.get('numRepresentantesRecorrido').setValue(String(this.representantes.length));
          this.openDialog('', '<b>La información se ha eliminado correctamente.</b>');
        } else {
          this.representantes.removeAt(numeroRepresentante);
          this.registerProjectETCService
            .deleteRepresentanteEtcRecorrido(representanteEtcid, this.representantes.length)
            .subscribe(
              (response: Respuesta) => {
                this.openDialog('', '<b>La información se ha eliminado correctamente.</b>');
                this.callOnInitParent.emit();
                return;
              },
              err => this.openDialog('', `<b>${err.message}</b>`)
            );
        }
      }
    });
  }

  arrayOne(n: number): any[] {
    return Array(n);
  }
}
