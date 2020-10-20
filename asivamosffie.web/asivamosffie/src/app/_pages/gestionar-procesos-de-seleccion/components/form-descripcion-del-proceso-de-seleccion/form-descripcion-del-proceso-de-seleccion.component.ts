import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormArray } from '@angular/forms';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { forkJoin, timer } from 'rxjs';
import { ProcesoSeleccion, ProcesoSeleccionGrupo, ProcesoSeleccionCronograma, ProcesoSeleccionService, TiposProcesoSeleccion } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
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
  tiposProceso=TiposProcesoSeleccion;

  editorStyle = {
    height: '100px',
    color: 'var(--mainColor)',
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };
  listaLimite: Dominio[]=[];
  listaSalarioMinimo: Dominio[]=[];
  

  constructor(
              private fb: FormBuilder,
              private commonService: CommonService,
              public dialog: MatDialog,
              private procesoSeleccionService: ProcesoSeleccionService,    
  ) { }

  ngOnInit() {

    this.addressForm = this.crearFormulario();
    console.log(this.addressForm.get("grupos"));
    return new Promise(resolve => {
      forkJoin([

        this.commonService.listaTipoIntervencion(),
        this.commonService.listaTipoAlcance(),
        this.commonService.listaPresupuestoProcesoSeleccion(),
        this.commonService.getUsuariosByPerfil(2),
        this.commonService.listaLimiteSalarios(),
        this.commonService.listaSalarios()

      ]).subscribe(respuesta => {

        this.listaTipoIntervencion = respuesta[0];
        this.listaTipoAlcance = respuesta[1];
        this.listatipoPresupuesto = respuesta[2];
        this.listaResponsables = respuesta[3];
        this.listaLimite = respuesta[4].filter(x=>x.codigo==this.procesoSeleccion.tipoProcesoCodigo);
        this.listaSalarioMinimo = respuesta[5];
        console.log("ha ver, reviso el salario minimo");
        console.log(this.listaLimite);

        

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
    console.log(e.editor.getLength()+" "+n);
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  validarGruposDiligenciados(): boolean {
    let diligenciado: boolean = false;

    this.grupos.controls.forEach( control => {
      /*let nombreGrupo: string = control.get('nombreGrupo').value ? control.get('nombreGrupo').value : '';
      let plazoGrupo:  string = control.get('plazoMeses').value ? control.get('plazoMeses').value : '';

      if (nombreGrupo.length > 0 || plazoGrupo.length > 0){
        console.log( nombreGrupo.length, plazoGrupo.length )
        diligenciado = true;
      }*/
      if(control.get('nombreGrupo').value!="" || control.get('nombreGrupo').value!=null)
      {
        diligenciado=true;
      }
      if(control.get('plazoMeses').value!="" || control.get('plazoMeses').value!=null)
      {
        diligenciado=true;
      }

    })
    
    return diligenciado;
  }

  CambioNumeroMeses(i:number)
  {
    console.log(this.addressForm.controls.grupos.value[i]);
    if(this.addressForm.controls.grupos.value[i].plazoMeses!="")
    {
      if(this.addressForm.controls.grupos.value[i].plazoMeses<=0)
      {
        this.openDialog("","<b>La cantidad de meses no puede ser 0.</b>");
        return;
      }
    }
    
  }

  CambioNumeroAportantes() {

    const FormGrupos = this.addressForm.value;
    if(FormGrupos.cuantosGrupos<=1)
    {
      this.openDialog("","<b>La cantidad de grupos no puede ser menor a 2.</b>");
      return;
    }
    if(FormGrupos.cuantosGrupos>=1)
    {
      if (FormGrupos.cuantosGrupos < this.grupos.length)
        if (this.validarGruposDiligenciados()){
          this.openDialog('','<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>');

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
    
    
  }

  createGrupo(): FormGroup {
    return this.fb.group({
      procesoSeleccionGrupoId: [],
      nombreGrupo: [null, Validators.compose([
        Validators.required, Validators.minLength(1), Validators.maxLength(100)])
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
    //si tiene id lo envio al servicio de eliminar
    console.log(borrarForm);

    if(borrarForm.value[0].procesoSeleccionGrupoId>0)
    {
      this.procesoSeleccionService.deleteProcesoSeleccionGrupoByID(borrarForm.value[0].procesoSeleccionGrupoId).subscribe();
    }
    this.addressForm.get('cuantosGrupos').setValue( this.grupos.length );

  }

  borrarActividades(borrarForm: any, i: number) {
    
    borrarForm.removeAt(i);
    //si tiene id lo envio al servicio de eliminar

    if(borrarForm.value[0].procesoSeleccionCronogramaId>0)
    {
      this.procesoSeleccionService.deleteProcesoSeleccionActividadesByID(borrarForm.value[0].procesoSeleccionCronogramaId).subscribe();
    }    
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
    console.log(this.cronogramas[i].value);
    if(this.cronogramas[i].value.procesoSeleccionCronogramaId>0)
    {
      this.procesoSeleccionService.deleteProcesoSeleccionActividadesByID(this.cronogramas[i].value.procesoSeleccionCronogramaId).subscribe();
    }
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
      condicionesFinancieras: [null, Validators.required],
      condicionesTecnicas: [null, Validators.required],
      condicionesJuridicas: [null, Validators.required],
      condicionesAsignacion: [null, Validators.required],
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
      this.procesoSeleccion.esDistribucionGrupos = this.addressForm.get('distribucionEnGrupos').value ? this.addressForm.get('distribucionEnGrupos').value : null,
      this.procesoSeleccion.responsableTecnicoUsuarioId = this.addressForm.get('responsableEquipoTecnico').value ? this.addressForm.get('responsableEquipoTecnico').value.usuarioId : null,
      this.procesoSeleccion.responsableEstructuradorUsuarioid = this.addressForm.get('responsableEquipoestructurado').value ? this.addressForm.get('responsableEquipoestructurado').value.usuarioId : null,
      this.procesoSeleccion.cantGrupos = this.addressForm.get('cuantosGrupos').value ? this.addressForm.get('cuantosGrupos').value : null,
      this.procesoSeleccion.procesoSeleccionGrupo = [],
      this.procesoSeleccion.procesoSeleccionCronograma = [];
      //para invitacion abierta
      this.procesoSeleccion.condicionesAsignacionPuntaje=this.addressForm.get('condicionesAsignacion').value?this.addressForm.get('condicionesAsignacion').value:null;
      this.procesoSeleccion.condicionesFinancierasHabilitantes=this.addressForm.get('condicionesFinancieras').value?this.addressForm.get('condicionesFinancieras').value:null;
      this.procesoSeleccion.condicionesJuridicasHabilitantes=this.addressForm.get('condicionesJuridicas').value?this.addressForm.get('condicionesJuridicas').value:null;
      this.procesoSeleccion.condicionesTecnicasHabilitantes=this.addressForm.get('condicionesTecnicas').value?this.addressForm.get('condicionesTecnicas').value:null;

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
      this.addressForm.get('distribucionEnGrupos').setValue(this.procesoSeleccion.esDistribucionGrupos?"true":"false");
      this.addressForm.get('cuantosGrupos').setValue(this.procesoSeleccion.cantGrupos);
      this.addressForm.get('condicionesJuridicas').setValue(this.procesoSeleccion.condicionesJuridicasHabilitantes);
      this.addressForm.get('condicionesFinancieras').setValue(this.procesoSeleccion.condicionesFinancierasHabilitantes);
      this.addressForm.get('condicionesTecnicas').setValue(this.procesoSeleccion.condicionesTecnicasHabilitantes);
      this.addressForm.get('condicionesAsignacion').setValue(this.procesoSeleccion.condicionesAsignacionPuntaje);      
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
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  nosuperarlimite(i:number,caso:number)
  {
    let limite=this.listaLimite[0].nombre.split(",");
    console.log(limite[1]);
    let maximo=parseInt(limite[1])*parseInt(this.listaSalarioMinimo[0].descripcion);
    let minimo=parseInt(limite[0])*parseInt(this.listaSalarioMinimo[0].descripcion);
    const listaGrupo = this.addressForm.get('grupos') as FormArray;
    if(caso==1)
    {
      if(this.addressForm.controls.grupos.value[i].valor>maximo
        ||
        this.addressForm.controls.grupos.value[i].valor<minimo)
      {        
        
        console.log(listaGrupo.controls[i]);
        listaGrupo.controls[i].get("valor").setValue(0);
        this.openDialog("","<b>El valor de salarios mínimos no corresponde con el tipo de proceso de selección. Verifique por favor.</b>");
      }      
    }
    else if(caso==2)
    {
      if(this.addressForm.controls.grupos.value[i].valorMaximoCategoria>maximo
        ||
        this.addressForm.controls.grupos.value[i].valorMaximoCategoria<minimo)
      {
        listaGrupo.controls[i].get("valorMaximoCategoria").setValue(0);
        this.openDialog("","<b>El valor de salarios mínimos no corresponde con el tipo de proceso de selección. Verifique por favor.</b>");
      }
    }
    else{
      if(this.addressForm.controls.grupos.value[i].valorMinimoCategoria > maximo
        ||
        this.addressForm.controls.grupos.value[i].valorMinimoCategoria < minimo)
      {
        listaGrupo.controls[i].get("valorMinimoCategoria").setValue(0);
        this.openDialog("","<b>El valor de salarios mínimos no corresponde con el tipo de proceso de selección. Verifique por favor.</b>");
      }
    }
  }
}
