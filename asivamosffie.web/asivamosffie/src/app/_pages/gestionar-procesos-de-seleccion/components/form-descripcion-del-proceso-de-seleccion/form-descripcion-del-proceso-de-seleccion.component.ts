import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormArray } from '@angular/forms';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { forkJoin, timer } from 'rxjs';
import { ProcesoSeleccion, ProcesoSeleccionGrupo, ProcesoSeleccionCronograma, ProcesoSeleccionService } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
import { delay } from 'rxjs/operators';
import { promise } from 'protractor';
import { resolve } from 'dns';
import { Usuario } from 'src/app/core/_services/autenticacion/autenticacion.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';


@Component({
  selector: 'app-form-descripcion-del-proceso-de-seleccion',
  templateUrl: './form-descripcion-del-proceso-de-seleccion.component.html',
  styleUrls: ['./form-descripcion-del-proceso-de-seleccion.component.scss']
})

export class FormDescripcionDelProcesoDeSeleccionComponent implements OnInit {

  @Input() procesoSeleccion: ProcesoSeleccion;
  @Output() guardar: EventEmitter<any> = new EventEmitter();

  listaTipoIntervencion: Dominio[];
  listaTipoAlcance: Dominio[];
  listatipoPresupuesto: Dominio[];
  listaResponsables: Usuario[];
  addressForm = this.fb.group({});

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

  constructor(
              private fb: FormBuilder,
              private commonService: CommonService,
              public dialog: MatDialog,    
  ) { }

  ngOnInit() {

    this.addressForm = this.crearFormulario();

    return new Promise(resolve => {
      forkJoin([

        this.commonService.listaTipoIntervencion(),
        this.commonService.listaTipoAlcance(),
        this.commonService.listaPresupuestoProcesoSeleccion(),
        this.commonService.getUsuariosByPerfil(2),

      ]).subscribe(respuesta => {

        this.listaTipoIntervencion = respuesta[0];
        this.listaTipoAlcance = respuesta[1];
        this.listatipoPresupuesto = respuesta[2];
        this.listaResponsables = respuesta[3];

        this.listaTipoAlcance = this.listaTipoAlcance.filter( t => t.codigo == "1" || t.codigo == "2" || t.codigo == "3" )

        resolve();

      });
    });
  }

  openDialog(modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
  }

  get grupos() {
    return this.addressForm.get('grupos') as FormArray;
  }

  get cronogramas() {
    return this.addressForm.get('cronogramas') as FormArray;
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  validarGruposDiligenciados(): boolean {
    let diligenciado: boolean = false;

    this.grupos.controls.forEach( control => {
      let nombreGrupo: string = control.get('nombreGrupo').value ? control.get('nombreGrupo').value : '';
      let plazoGrupo:  string = control.get('plazoMeses').value ? control.get('plazoMeses').value : '';

      if (nombreGrupo.length > 0 || plazoGrupo.length > 0){
        console.log( nombreGrupo.length, plazoGrupo.length )
        diligenciado = true;
      }

    })
    
    return diligenciado;
  }

  CambioNumeroAportantes() {

    const FormGrupos = this.addressForm.value;

    if (FormGrupos.cuantosGrupos < this.grupos.length)
      if (this.validarGruposDiligenciados()){
        this.openDialog('','Debe eliminar uno de los registros diligenciados para disminuir el total de los registros   requeridos');

        this.addressForm.get('cuantosGrupos').setValue( this.grupos.length );
        
        return false;
      }

    if (FormGrupos.cuantosGrupos > this.grupos.length && FormGrupos.cuantosGrupos < 100) {
      while (this.grupos.length < FormGrupos.cuantosGrupos) {
        this.grupos.push(this.createGrupo());
      }
    } else if (FormGrupos.cuantosGrupos <= this.grupos.length && FormGrupos.cuantosGrupos >= 0) {
      while (this.grupos.length > FormGrupos.cuantosGrupos) {
        this.borrarArray(this.grupos, this.grupos.length - 1);
      }
    }
    
  }

  createGrupo(): FormGroup {
    return this.fb.group({
      procesoSeleccionGrupoId: [],
      nombreGrupo: [null, Validators.compose([
        Validators.required, Validators.minLength(5), Validators.maxLength(100)])
      ],
      tipoPresupuesto: [null, Validators.required],
      valor: [],
      valorMinimoCategoria: [],
      valorMaximoCategoria: [],
      plazoMeses: [null, Validators.compose([
        Validators.required, Validators.minLength(1), Validators.maxLength(2)])
      ]
    });
  }

  borrarArray(borrarForm: any, i: number) {
    
    borrarForm.removeAt(i);

    this.addressForm.get('cuantosGrupos').setValue( this.grupos.length );

  }

  agregarActividad() {
    this.cronogramas.push(this.createActividad());
  }

  createActividad(): FormGroup {
    return this.fb.group({
      procesoSeleccionCronogramaId: [],
      descripcion: [null, Validators.required],
      fechaMaxima: [null, Validators.required]
    });
  }

  borrarCronograma( i: number ){
    this.cronogramas.removeAt(i);
  }

  crearFormulario() {
    // return //this.addressForm =
    return this.fb.group({
      objeto: [null, Validators.required],
      alcanceParticular: [null, Validators.required],
      justificacion: [null, Validators.required],
      criterios: [null, Validators.required],
      tipoIntervencion: [null, Validators.required],
      tipoAlcance: [null, Validators.required],
      distribucionEnGrupos: ['', Validators.required],
      cuantosGrupos: [null, Validators.compose([
        Validators.minLength(1), Validators.maxLength(2)])
      ],
      responsableEquipoTecnico: [],
      responsableEquipoestructurado: [],
      grupos: this.fb.array([
        this.fb.group({
          procesoSeleccionGrupoId: [],
          nombreGrupo: [null, Validators.compose([
            Validators.required, Validators.minLength(5), Validators.maxLength(100)])
          ],
          tipoPresupuesto: [null, Validators.required],
          valor: [],
          valorMinimoCategoria: [],
          valorMaximoCategoria: [],
          plazoMeses: [null, Validators.compose([
            Validators.required, Validators.minLength(1), Validators.maxLength(2)])
          ]
        })
      ]),
      cronogramas: this.fb.array([
        this.fb.group({
          procesoSeleccionCronogramaId: [],
          descripcion: [null, Validators.required],
          fechaMaxima: [null, Validators.required]
        })
      ]),
    });
  }

  createCronograma() {
    return this.fb.group({
      procesoSeleccionCronogramaId: [],
      descripcion: [null, Validators.required],
      fechaMaxima: [null, Validators.required]
    });
  }

  textoLimpio(texto: string) {
    const textolimpio = texto.replace(/<[^>]*>/g, '');
    return textolimpio.length;
  }

  onSubmit() {
    console.log(this.addressForm.value);

    const listaGrupos = this.addressForm.get('grupos') as FormArray;
    const listaCronogramas = this.addressForm.get('cronogramas') as FormArray;

    this.procesoSeleccion.objeto = this.addressForm.get('objeto').value,
      this.procesoSeleccion.alcanceParticular = this.addressForm.get('alcanceParticular').value,
      this.procesoSeleccion.justificacion = this.addressForm.get('justificacion').value,
      this.procesoSeleccion.criteriosSeleccion = this.addressForm.get('criterios').value,
      this.procesoSeleccion.tipoIntervencionCodigo = this.addressForm.get('tipoIntervencion').value ? this.addressForm.get('tipoIntervencion').value.codigo : null,
      this.procesoSeleccion.tipoAlcanceCodigo = this.addressForm.get('tipoAlcance').value ? this.addressForm.get('tipoAlcance').value.codigo : null,
      this.procesoSeleccion.esDistribucionGrupos = this.addressForm.get('distribucionEnGrupos').value ? this.addressForm.get('distribucionEnGrupos').value.codigo : null,
      this.procesoSeleccion.responsableTecnicoUsuarioId = this.addressForm.get('responsableEquipoTecnico').value ? this.addressForm.get('responsableEquipoTecnico').value.usuarioId : null,
      this.procesoSeleccion.responsableEstructuradorUsuarioid = this.addressForm.get('responsableEquipoestructurado').value ? this.addressForm.get('responsableEquipoestructurado').value.usuarioId : null,
      this.procesoSeleccion.procesoSeleccionGrupo = [],
      this.procesoSeleccion.procesoSeleccionCronograma = [];

    this.procesoSeleccion.procesoSeleccionGrupo = [];

    listaGrupos.controls.forEach(control => {
      const grupo: ProcesoSeleccionGrupo = {
        nombreGrupo: control.get('nombreGrupo').value,

        plazoMeses: control.get('plazoMeses').value,
        procesoSeleccionId: this.procesoSeleccion.procesoSeleccionId,
        procesoSeleccionGrupoId: control.get('procesoSeleccionGrupoId').value,
        tipoPresupuestoCodigo: control.get('tipoPresupuesto').value ? control.get('tipoPresupuesto').value.codigo : null,
        valor: control.get('valor').value,
        valorMinimoCategoria: control.get('valorMinimoCategoria').value,
        valorMaximoCategoria: control.get('valorMaximoCategoria').value,
      };
      this.procesoSeleccion.procesoSeleccionGrupo.push(grupo);
    });

    this.procesoSeleccion.procesoSeleccionCronograma = [];

    let posicion = 1;
    listaCronogramas.controls.forEach(control => {
      const cronograma: ProcesoSeleccionCronograma = {
        descripcion: control.get('descripcion').value,
        fechaMaxima: control.get('fechaMaxima').value,
        numeroActividad: posicion,
        procesoSeleccionCronogramaId: control.get('procesoSeleccionCronogramaId').value,
        procesoSeleccionId: this.procesoSeleccion.procesoSeleccionId,
        estadoActividadCodigo: null,
      };
      this.procesoSeleccion.procesoSeleccionCronograma.push(cronograma);
      posicion++;
    });

    console.log(this.procesoSeleccion);
    this.guardar.emit(null);
  }

  cargarRegistro() {


    this.ngOnInit().then(() => {
      const tipoIntervencion = this.listaTipoIntervencion.find(t => t.codigo === this.procesoSeleccion.tipoIntervencionCodigo);
      const tipoAlcance = this.listaTipoAlcance.find(a => a.codigo === this.procesoSeleccion.tipoAlcanceCodigo);
      const responsableEquipoTecnico = this.listaResponsables.find(a => a.usuarioId === this.procesoSeleccion.responsableTecnicoUsuarioId);
      const responsableEquipoestructurado = this.listaResponsables.find(a => a.usuarioId === this.procesoSeleccion.responsableEstructuradorUsuarioid);
      const listaGrupo = this.addressForm.get('grupos') as FormArray;
      const listaCronograma = this.addressForm.get('cronogramas') as FormArray;

      listaGrupo.clear();
      listaCronograma.clear();

      this.addressForm.get('objeto').setValue(this.procesoSeleccion.objeto);
      this.addressForm.get('alcanceParticular').setValue(this.procesoSeleccion.alcanceParticular);
      this.addressForm.get('justificacion').setValue(this.procesoSeleccion.justificacion);
      this.addressForm.get('criterios').setValue(this.procesoSeleccion.criteriosSeleccion);
      this.addressForm.get('tipoIntervencion').setValue(tipoIntervencion);
      this.addressForm.get('tipoAlcance').setValue(tipoAlcance);
      this.addressForm.get('distribucionEnGrupos').setValue(this.procesoSeleccion.esDistribucionGrupos ? this.procesoSeleccion.esDistribucionGrupos.toString() : null);

      this.addressForm.get('responsableEquipoTecnico').setValue(responsableEquipoTecnico);
      this.addressForm.get('responsableEquipoestructurado').setValue(responsableEquipoestructurado);

      this.procesoSeleccion.procesoSeleccionGrupo.forEach(grupo => {
        const control = this.createGrupo();
        const tipoPresupuestoValor = this.listatipoPresupuesto.find(p => p.codigo === grupo.tipoPresupuestoCodigo);

        control.get('nombreGrupo').setValue(grupo.nombreGrupo);
        control.get('plazoMeses').setValue(grupo.plazoMeses);
        control.get('procesoSeleccionGrupoId').setValue(grupo.procesoSeleccionGrupoId);
        control.get('tipoPresupuesto').setValue(tipoPresupuestoValor);
        control.get('valor').setValue(grupo.valor);
        control.get('valorMinimoCategoria').setValue(grupo.valorMinimoCategoria);
        control.get('valorMaximoCategoria').setValue(grupo.valorMaximoCategoria);

        listaGrupo.push(control);
      });

      this.procesoSeleccion.procesoSeleccionCronograma.forEach(cronograma => {
        const control = this.createCronograma();

        control.get('descripcion').setValue(cronograma.descripcion),
          control.get('fechaMaxima').setValue(cronograma.fechaMaxima),
          control.get('procesoSeleccionCronogramaId').setValue(cronograma.procesoSeleccionCronogramaId);

        listaCronograma.push(control);
      });
    });
  }
}
