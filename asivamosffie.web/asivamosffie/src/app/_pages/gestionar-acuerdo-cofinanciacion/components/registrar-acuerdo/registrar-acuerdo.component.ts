import { Component, OnInit } from '@angular/core';
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

  constructor(private fb: FormBuilder,
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
  tiposPersonaHabilitaNombre: string[] = ['3'];

  datosAportantes = this.fb.group({
    vigenciaEstado: ['', Validators.required],
    numAportes: ['', [Validators.required, Validators.maxLength(3), Validators.min(1), Validators.max(999)]],
    aportantes: this.fb.array([])
  });

  // tabla de los documentos de aportantes
  documentoApropiacion: CofinanciacionDocumento[] = [];

  EditMode() {
    this.activatedRoute.params.subscribe(param => {


      if (param.id) {
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

            this.commonService.listaMunicipiosByIdDepartamento(apor.municipioId.toString().substring(0, 5)).subscribe(mun => {

              const valorMunicipio = mun.find(a => a.localizacionId === apor.municipioId.toString());
              const valorDepartamento = this.departamentos.find(a => a.localizacionId === apor.municipioId.toString().substring(0, 5));

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

  changeTipoAportante(p) {
    console.log(p);
  }

  CambioNumeroAportantes() {    
    const FormNumAportantes = this.datosAportantes.value;
    if (FormNumAportantes.numAportes > this.aportantes.length && FormNumAportantes.numAportes < 1000) {
      while (this.aportantes.length < FormNumAportantes.numAportes) {
        this.aportantes.push(this.createAportante());  
      }
    } else if (FormNumAportantes.numAportes <= this.aportantes.length && FormNumAportantes.numAportes >= 0) {
      while (this.aportantes.length > FormNumAportantes.numAportes) {
        //this.borrarAportante(this.aportantes, this.aportantes.length - 1);
        // this.listaCofinancAportantes.pop();
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
    borrarForm.removeAt(i);
    const index = this.listaCofinancAportantes.indexOf(this.listaCofinancAportantes[i]);
    this.listaCofinancAportantes.splice(index, 1);
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
        cofinanciacionId: control.get('cofinanciacionId').value,
        cofinanciacionAportanteId: control.get('cofinanciacionAportanteId').value,
        cofinanciacionDocumento
      };

      listaAportantesTemp.push(cofiApo);
      // this.listaCofinancAportantes.push(cofiApo);
      i++;
    });
    this.listaCofinancAportantes = listaAportantesTemp;
    // this.mostrarDocumentosDeApropiacion = true;
  }

  getAportantes() {
    return;
  }

  onSubmit() {
    this.loading = true;
    // console.log('entró');
    // this.listaCofinancAportantes = [];
    this.listaAportantes();
    const cofinanciacion: Cofinanciacion =
    {
      vigenciaCofinanciacionId: this.datosAportantes.get('vigenciaEstado').value,
      cofinanciacionAportante: this.listaCofinancAportantes,
      cofinanciacionId: this.id
    };

    console.log(cofinanciacion);

    this.cofinanciacionService.CrearOModificarAcuerdoCofinanciacion(cofinanciacion).subscribe(
      respuesta => {
        this.verificarRespuesta(respuesta);
      },
      err => {
        let mensaje: string;
        if (err.error.message) {
          mensaje = err.error.message;
        } else {
          mensaje = err.message;
        }
        this.loading = false;
        this.openDialog('Error', mensaje);
      },
      () => {
        // console.log('terminó');
        this.loading = false;
      });
  }

  private verificarRespuesta(respuesta: Respuesta) {
    if (respuesta.isSuccessful) // Response witout errors
    {
      this.openDialog('', respuesta.message);
      if (!respuesta.isValidation) // have validations
      {
        this.router.navigate(['/gestionarAcuerdos']);
      }
    } else {
      this.openDialog('', respuesta.message);
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
      while (this.listaCofinancAportantes[identificador].cofinanciacionDocumento.length > cantidadDocumentos) {
        this.listaCofinancAportantes[identificador].cofinanciacionDocumento.pop();
      }
    }




  }

  validaCompletitud(aportante: any) {

    let retorno = 0;
    aportante.cofinanciacionDocumento.forEach(element => {
      if (
        element.fechaAcuerdo == null &&
        element.numeroAcuerdo == null &&
        element.tipoDocumentoId == null &&
        element.valorDocumento == null &&
        // element.valorTotalAportante==null&&
        element.vigenciaAporte == null) {
        // retorno=0;
      }
      else if (
        element.fechaAcuerdo != null &&
        element.numeroAcuerdo != null &&
        element.tipoDocumentoId != null &&
        element.valorDocumento != null &&
        // element.valorTotalAportante!=null&&
        element.vigenciaAporte != null) {
        retorno += 2;
      }
      else {
        retorno++;
      }
    });
    const resultado = aportante.cofinanciacionDocumento.length * 2 == retorno ? 3 : retorno == 0 ? 1 : 2;
    return resultado;
  }
  getTotalAportante(aportante: any) {
    let retorno = 0;
    aportante.cofinanciacionDocumento.forEach(element => {
      if (element) {
        retorno += element.valorDocumento;
      }

    });
    return retorno;
  }
  eliminadoc(aportante: any, documento: any) {
    console.log(aportante);
    console.log(documento);
    const index = aportante.cofinanciacionDocumento.indexOf(documento, 0);
    if (index > -1) {
      aportante.cofinanciacionDocumento.splice(index, 1);
      aportante.cauntosDocumentos--;
    }

  }

  // evalua tecla a tecla
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  validateNumber(event: Event, max) {
    console.log(event);
    const alphanumeric = /[0-9]/;
    // let inputChar = String.fromCharCode(event.charCode);
    // return alphanumeric.test(inputChar) ? true : false;
  }
}
