import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, Validators, FormArray, ControlValueAccessor, FormGroup, FormControl, FormsModule, ControlContainer } from '@angular/forms';
import { CofinanciacionService, CofinanciacionAportante, Cofinanciacion, CofinanciacionDocumento } from 'src/app/core/_services/Cofinanciacion/cofinanciacion.service';
import { Dominio, CommonService, Respuesta, Localizacion } from 'src/app/core/_services/common/common.service';
import { ClassGetter } from '@angular/compiler/src/output/output_ast';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-registrar-acuerdo',
  templateUrl: './registrar-acuerdo.component.html',
  styleUrls: ['./registrar-acuerdo.component.scss']
})
export class RegistrarAcuerdoComponent implements OnInit {

  loading = false;

  estaEditando = false;

  constructor(
    private fb: FormBuilder,
    private cofinanciacionService: CofinanciacionService,
    private commonService: CommonService,
    public dialog: MatDialog,
    private activatedRoute: ActivatedRoute,
    private router: Router,
  ) {
    this.maxDate = new Date();
  }

  get aportantes() {
    return this.datosAportantes.get('aportantes') as FormArray;
  }

  get DocumentoAportantes() {
    return this.documentoApropiacion;
  }

  mostrarDocumentosDeApropiacion = false;
  maxDate: Date;
  vigenciaEstados: number[];
  vigenciasAportante: number[];
  tiposDocumento: Dominio[];
  selectTiposAportante: Dominio[];
  nombresAportante: Dominio[];
  departamentos: Localizacion[];
  valorTotalAcuerdo = 0;
  listaCofinancAportantes: CofinanciacionAportante[] = [];
  id = 0;
  tiposPersonaHabilitaNombre: string[] = ['2'];

  datosAportantes = this.fb.group({
    vigenciaEstado: ['', Validators.required],
    numAportes: ['', [Validators.required, Validators.maxLength(3), Validators.min(1), Validators.max(999)]],
    aportantes: this.fb.array([])
  });
  noGuardado=true;
  ngOnDestroy(): void {
    if ( this.noGuardado===true && this.datosAportantes.dirty) {
      let dialogRef =this.dialog.open(ModalDialogComponent, {
        width: '28em',
        data: { modalTitle:"", modalText:"¿Desea guardar la información registrada?",siNoBoton:true }
      });   
      dialogRef.afterClosed().subscribe(result => {
        console.log(`Dialog result: ${result}`);
        if(result === true)
        {
            this.onSave(false);          
        }           
      });
    }
  };

  // tabla de los documentos de aportantes
  documentoApropiacion: CofinanciacionDocumento[] = [];

  EditMode() {
    this.activatedRoute.params.subscribe(param => {

      if (param.id) {
        this.estaEditando = true;
        this.id = param.id;
        this.cofinanciacionService.getAcuerdoCofinanciacionById(this.id).subscribe(cof => {
          this.mostrarDocumentosDeApropiacion = true;
          this.datosAportantes.setControl('vigenciaEstado', this.fb.control(cof.vigenciaCofinanciacionId, Validators.required));
          this.datosAportantes.setControl('numAportes', this.fb.control(cof.cofinanciacionAportante.length));

          cof.cofinanciacionAportante.forEach(apor => {
            const grupo: FormGroup = this.createAportanteEditar(apor.tipoAportanteId,
              apor.nombreAportanteId,
              apor.cofinanciacionDocumento.length,
              apor.cofinanciacionId,
              apor.cofinanciacionAportanteId);

            const valorTipo = this.selectTiposAportante.find(a => a.dominioId === apor.tipoAportanteId);
            const valorNombre = this.nombresAportante.find(a => a.dominioId === apor.nombreAportanteId);
            const idMunicipio = apor.municipioId ? apor.municipioId.toString() : '00000';
            const idDepartamento = apor.departamentoId ? apor.departamentoId.toString() : '000';

            this.commonService.listaMunicipiosByIdDepartamento(idMunicipio.substring(0, 5)).subscribe(mun => {

              const valorMunicipio = mun.find(a => a.localizacionId === idMunicipio);
              const valorDepartamento = this.departamentos.find(a => a.localizacionId === idDepartamento);

              grupo.get('departamento').setValue(valorDepartamento);
              grupo.get('municipios').setValue(mun);
              grupo.get('municipio').setValue(valorMunicipio);
              grupo.get('tipo').setValue(valorTipo);
              grupo.get('nombre').setValue(valorNombre);

              this.aportantes.push(grupo);
            });

          });

          this.listaCofinancAportantes = cof.cofinanciacionAportante;
          this.actualizarValores();

        });
      }
    });
  }

  actualizarValores() {
    let valorTotal = 0;
    this.listaCofinancAportantes.forEach(apo => {
      let valorAportante = 0;
      apo.cofinanciacionDocumento.forEach(doc => {
        console.log(doc.valorDocumento)
        valorTotal += doc.valorDocumento ? doc.valorDocumento : 0;
        valorAportante += doc.valorDocumento ? doc.valorDocumento : 0;
      });
      apo.valortotal = valorAportante;
    });

    this.valorTotalAcuerdo = valorTotal;
  }

  ngOnInit(): void {

    this.vigenciasAportante = this.cofinanciacionService.vigenciasAcuerdoCofinanciacion();
    this.vigenciaEstados = this.cofinanciacionService.vigenciasAcuerdoCofinanciacion();

    forkJoin
      ([
        this.commonService.listaTipoDocFinanciacion(),
        this.commonService.listaTipoAportante(),
        this.commonService.listaNombreAportante(),
        this.commonService.listaDepartamentos(),
      ]).subscribe
      (
        res => {
          this.tiposDocumento = res[0];
          this.selectTiposAportante = res[1];
          this.nombresAportante = res[2];
          this.departamentos = res[3];
          this.EditMode();
        }
      );
  }

  changeDepartamento(id: string | number) {
    this.commonService.listaMunicipiosByIdDepartamento(this.aportantes.controls[id]
      .get('departamento').value.localizacionId).subscribe(mun => {
        this.aportantes.controls[id].get('municipios').setValue(mun);
      });
  }


  changeTipoAportante(p: any) {
    console.log(p);
  }

  borrarArray(borrarForm: any, i: number) {

    borrarForm.removeAt(i);

    // this.addressForm.get('cuantosGrupos').setValue( this.grupos.length );

  }

  CambioNumeroAportantes() {
    const FormNumAportantes = this.datosAportantes.value;
    if (FormNumAportantes.numAportes !== null) {
      if (FormNumAportantes.numAportes === 0) {
        this.openDialog('', '<b>La cantidad de aportantes no puede ser igual a 0.</b>');
        return;
      }
      if (FormNumAportantes.numAportes > this.aportantes.length && FormNumAportantes.numAportes < 1000) {
        while (this.aportantes.length < FormNumAportantes.numAportes) {
          this.aportantes.push(this.createAportante());
        }
      } else if (FormNumAportantes.numAportes <= this.aportantes.length && FormNumAportantes.numAportes >= 0) {

        console.log(this.datosAportantes);
        console.log(this.datosAportantes.value.numAportes);
        let estaenblanco = true;
        this.datosAportantes.value.aportantes.forEach((element: {
          cauntosDocumentos: string;
          cofinanciacionAportanteId: string;
          cofinanciacionId: string;
          departamento: string;
          municipio: string;
          municipios: string;
          nombre: string;
          tipo: string;
        }) => {
          if (element.cauntosDocumentos !== '' ||
            element.cofinanciacionAportanteId !== '' ||
            element.cofinanciacionId !== '' ||
            element.departamento !== '' ||
            element.municipio !== '' ||
            element.municipios !== '' ||
            element.nombre !== '' ||
            element.tipo !== '') {
            estaenblanco = false;
          }
        });
        if (estaenblanco) {

          while (this.aportantes.length > FormNumAportantes.numAportes) {
            this.borrarArray(this.aportantes, this.aportantes.length - 1);
            this.listaCofinancAportantes.pop();
          }
        }
        else {
          this.openDialog(
            '',
            '<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>'
          );
          this.datosAportantes.controls.numAportes.setValue(this.aportantes.length);
        }
      }
    }
  }

  createAportante(): FormGroup {

    const grupo: FormGroup = this.fb.group({
      tipo: ['', Validators.required],
      nombre: [''],
      cauntosDocumentos: ['', [Validators.required, Validators.maxLength(3), Validators.min(1), Validators.max(999)]],
      cofinanciacionId: [''],
      cofinanciacionAportanteId: [''],
      departamento: [''],
      municipio: [''],
      municipios: ['']
    });


    const cofiApo: CofinanciacionAportante = {
      tipoAportanteId: '',
      nombreAportanteId: '',
      municipioId: 0,
      departamentoId: 0,
      cofinanciacionId: 0,
      cofinanciacionAportanteId: 0,
      cofinanciacionDocumento: []
    };
    this.listaCofinancAportantes.push(cofiApo);

    return grupo;

  }

  createAportanteEditar(pTipo: number, pNombre: number, pCantidad: number, pCofinanciacionId: number, pCofinanciacionAportanteId: number)
    : FormGroup {
    const grupo: FormGroup = this.fb.group({
      tipo: [pTipo, Validators.required],
      nombre: [pNombre],
      cauntosDocumentos: [pCantidad, [Validators.required, Validators.maxLength(3), Validators.min(1), Validators.max(999)]],
      cofinanciacionId: [pCofinanciacionId],
      cofinanciacionAportanteId: [pCofinanciacionAportanteId],
      departamento: [''],
      municipio: [''],
      municipios: ['']
    });

    for (let i = 0; i < pCantidad; i++) {
      const cofiApo: CofinanciacionAportante = {
        tipoAportanteId: '',
        nombreAportanteId: '',
        municipioId: 0,
        departamentoId: 0,
        cofinanciacionId: 0,
        cofinanciacionAportanteId: 0,
        cofinanciacionDocumento: []
      };
      this.listaCofinancAportantes.push(cofiApo);
    }

    return grupo;

  }

  createDocumentoAportante(): FormGroup {

    return this.fb.group({
      vigenciaAportante: [null, Validators.required],
      valorIndicadoEnElDocumento: [null, Validators.compose([
        Validators.required, Validators.minLength(4), Validators.maxLength(20)])
      ],
      tipoDocumento: [null, Validators.required],
      numeroDocumento: [null, Validators.compose([
        Validators.required, Validators.minLength(10), Validators.maxLength(10)])
      ],
      fechaDocumento: [null, Validators.required]
    });
  }


  borrarAportante(borrarForm: any, i: number) {
    // antes de borrar reviso si tiene id, pues se debe borrar de manera logica
    if (borrarForm.value[i].cofinanciacionAportanteId > 0) {
      this.cofinanciacionService
        .EliminarCofinanciacionAportanteByCofinanciacionAportanteId(borrarForm.value[i].cofinanciacionAportanteId).subscribe(
          result => { location.reload(); }
        );
    }

    borrarForm.removeAt(i);
    const index = this.listaCofinancAportantes.indexOf(this.listaCofinancAportantes[i]);
    this.listaCofinancAportantes.splice(index, 1);
    this.datosAportantes.controls.numAportes.setValue(this.aportantes.length);
  }

  listaAportantes() {
    const listaAportantesTemp = [];
    let i = 0;
    const listaCofinancAportant: CofinanciacionAportante[] = [];
    this.aportantes.controls.forEach(control => {
      let cofinanciacionDocumento = [];
      if (this.listaCofinancAportantes[i]) {
        if (this.listaCofinancAportantes[i].cofinanciacionDocumento.length > 0) {
          cofinanciacionDocumento = this.listaCofinancAportantes[i].cofinanciacionDocumento;
        }
      }
      const cofiApo: CofinanciacionAportante = {
        tipoAportanteId: control.get('tipo').value ? control.get('tipo').value.dominioId : null,
        nombreAportanteId: control.get('nombre').value ? control.get('nombre').value.dominioId : null,
        municipioId: control.get('municipio').value ? control.get('municipio').value.localizacionId : null,
        departamentoId: control.get('departamento').value ? control.get('departamento').value.localizacionId : null,
        cofinanciacionId: control.get('cofinanciacionId').value,
        cofinanciacionAportanteId: control.get('cofinanciacionAportanteId').value,
        cofinanciacionDocumento
      };

      listaAportantesTemp.push(cofiApo);
      i++;
    });
    this.listaCofinancAportantes = listaAportantesTemp;
    // this.mostrarDocumentosDeApropiacion = true;
  }

  getAportantes() {
    return;
  }

  onSave(parcial: boolean) {
    this.loading = true;
    this.listaAportantes();

    const cofinanciacion: Cofinanciacion =
    {
      vigenciaCofinanciacionId: this.datosAportantes.get('vigenciaEstado').value,
      cofinanciacionAportante: this.listaCofinancAportantes,
      cofinanciacionId: this.id
    };

    this.cofinanciacionService.CrearOModificarAcuerdoCofinanciacion(cofinanciacion).subscribe(
      respuesta => {
        this.noGuardado=false;
        this.verificarRespuesta(respuesta, parcial);
      },
      err => {
        this.loading = false;
        let mensaje: string;
        if (err.error.message) {
          mensaje = err.error.message;
        } else {
          mensaje = err.message;
        }
       
        this.openDialog('Error', mensaje);
      },
      () => {
        // console.log('terminó');
        this.loading = false;
      });
  }

  private verificarRespuesta(respuesta: Respuesta, parcial: boolean) {
    if (respuesta.isSuccessful) // Response witout errors
    {
      this.openDialog('', `<b>${respuesta.message}</b>`);
      if (!respuesta.isValidation) // have validations
      {
        console.log(respuesta);
        if (parcial) {
          this.router.navigate([`/registrarAcuerdos/${respuesta.data.cofinanciacionId}`]);
        } else {
          this.router.navigate(['/gestionarAcuerdos']);
        }
      }
    } else {
      this.openDialog('', `<b>${respuesta.message}</b>`);
    }
  }

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  cantidadDocumentos(data: any, identificador: number) {

    this.mostrarDocumentosDeApropiacion = true;
    const cantidadDocumentos: number = data.get('cauntosDocumentos').value;
    if (cantidadDocumentos === 0 || cantidadDocumentos === null) {
      if (cantidadDocumentos === 0) {
        this.openDialog('', '<b>La cantidad de documentos de apropiación del aportante no puede ser igual a 0.</b>');
        return;
      }
    }
    else {
      console.log(cantidadDocumentos, ' ', this.listaCofinancAportantes[identificador].cofinanciacionDocumento.length);
      if (cantidadDocumentos > this.listaCofinancAportantes[identificador].cofinanciacionDocumento.length
        && cantidadDocumentos < 1000) {
        while (this.listaCofinancAportantes[identificador].cofinanciacionDocumento.length < cantidadDocumentos) {

          this.listaCofinancAportantes[identificador].cofinanciacionDocumento.push(
            {
              cofinanciacionAportanteId: identificador,
              fechaAcuerdo: null,
              numeroAcuerdo: null,
              cofinanciacionDocumentoId: null,
              numeroActa: null,
              fechaActa: null,
              tipoDocumentoId: null,
              valorDocumento: null,
              valorTotalAportante: null,
              vigenciaAporte: null
            });

        }
      } else if (cantidadDocumentos <= this.listaCofinancAportantes[identificador].cofinanciacionDocumento.length
        && cantidadDocumentos >= 0) {
        // para saber si borro, debo ver si tiene algo
        let estaenblanco = true;
        this.listaCofinancAportantes[identificador].cofinanciacionDocumento.forEach(element => {
          if (element.fechaAcuerdo !== null ||
            element.numeroAcuerdo !== null ||
            element.cofinanciacionDocumentoId !== null ||
            element.numeroActa !== null ||
            element.fechaActa !== null ||
            element.tipoDocumentoId !== null ||
            element.valorDocumento !== null ||
            element.valorTotalAportante !== null ||
            element.vigenciaAporte !== null) {
            estaenblanco = false;
          }
        });
        if (estaenblanco) {
          while (this.listaCofinancAportantes[identificador].cofinanciacionDocumento.length > cantidadDocumentos) {
            this.listaCofinancAportantes[identificador].cofinanciacionDocumento.pop();
          }
        }
        else {
          this.openDialog('', '<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>');
        }

      }
    }
  }

  validaCompletitud(aportante: any) {

    let retorno = 0;
    aportante.cofinanciacionDocumento
      .forEach((element: { fechaAcuerdo: any; numeroAcuerdo: any; tipoDocumentoId: any; valorDocumento: any; vigenciaAporte: any; }) => {
        
        if (
          (element.fechaAcuerdo === null &&
          element.numeroAcuerdo === null &&
          element.tipoDocumentoId === null &&
          element.valorDocumento === null &&
          // element.valorTotalAportante==null&&
          element.vigenciaAporte === null) ||
          (!element.fechaAcuerdo &&
            !element.numeroAcuerdo &&
            !element.tipoDocumentoId &&
            !element.valorDocumento &&
            
            !element.vigenciaAporte)) {

          // retorno=0;
        }
        else if (
          element.fechaAcuerdo !== null &&
          element.numeroAcuerdo !== null &&
          element.tipoDocumentoId !== null &&
          element.valorDocumento !== null &&
          // element.valorTotalAportante!=null&&
          element.vigenciaAporte !== null) {
          retorno += 2;
        }
        else {
          retorno++;
        }        
      });
    const resultado = aportante.cofinanciacionDocumento.length * 2 === retorno ? 3 : retorno === 0 ? 1 : 2;
    return resultado;
  }
  getTotalAportante(aportante: any) {
    let retorno = 0;
    aportante.cofinanciacionDocumento.forEach((element: { valorDocumento: number; }) => {
      if (element) {
        retorno += element.valorDocumento;
      }

    });
    return retorno;
  }
  eliminadoc(aportante: any, documento: any, indexd: any) {
    console.log(aportante);
    console.log(indexd);
    const index = aportante.cofinanciacionDocumento.indexOf(documento, 0);
    if (index > -1) {
      aportante.cofinanciacionDocumento.splice(index, 1);
    }
    aportante.cauntosDocumentos = aportante.cofinanciacionDocumento.length;
    this.datosAportantes.get('aportantes')['controls'][indexd]
      .controls.cauntosDocumentos.setValue(aportante.cofinanciacionDocumento.length);
  }

  // evalua tecla a tecla
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  validateNumber(event: Event, max: any) {
    console.log(event);
    const alphanumeric = /[0-9]/;
    // let inputChar = String.fromCharCode(event.charCode);
    // return alphanumeric.test(inputChar) ? true : false;
  }
}