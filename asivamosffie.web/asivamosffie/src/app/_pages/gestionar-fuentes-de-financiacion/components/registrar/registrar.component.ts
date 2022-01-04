import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, Validators, FormArray, ControlValueAccessor, FormGroup, FormControl } from '@angular/forms';
import { CommonService, Dominio, Localizacion, TiposAportante } from 'src/app/core/_services/common/common.service';
import {
  CofinanciacionService,
  CofinanciacionAportante,
  CofinanciacionDocumento
} from 'src/app/core/_services/Cofinanciacion/cofinanciacion.service';
import { forkJoin, from } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import {
  FuenteFinanciacionService,
  FuenteFinanciacion,
  CuentaBancaria,
  RegistroPresupuestal,
  VigenciaAporte,
  ControlRecurso
} from 'src/app/core/_services/fuenteFinanciacion/fuente-financiacion.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { CuentaBancariaService } from 'src/app/core/_services/cuentaBancaria/cuenta-bancaria.service';
import { mergeMap, tap, toArray } from 'rxjs/operators';
import { Respuesta } from 'src/app/core/_services/autenticacion/autenticacion.service';
import { Aportante } from 'src/app/core/_services/project/project.service';

@Component({
  selector: 'app-registrar',
  templateUrl: './registrar.component.html',
  styleUrls: ['./registrar.component.scss']
})
export class RegistrarComponent implements OnInit {

  listaVigenciasEliminadas=[];
  listaRpEliminados=[];
  listaFuentesEliminadas=[];
  estaEditando = false;

  addressForm: FormGroup;

  maxDate: Date;
  nombresAportantes: CofinanciacionAportante[] = [];
  fuentesDeRecursosLista: Dominio[] = [];
  bancos: Dominio[] = [];
  VigenciasAporte = [];
  departamentos: Localizacion[] = [];
  municipios: Localizacion[] = [];
  tipoAportanteId = 0;
  tipoAportante = TiposAportante;
  idAportante = 0;
  listaFuentes: FuenteFinanciacion[] = [];
  listaDocumentos: CofinanciacionDocumento[] = [];
  valorTotal = 0;
  valorRPTmp : number = 0;
  valorDocumentoTmp: number = 0;
  data;

  mostrarNombreaportante: boolean;
  listaDocumentosApropiacion: CofinanciacionDocumento[];
  tipoDocumentoap: Dominio[] = [];
  nombresAportantes2: CofinanciacionAportante[];
  solonombres: any[] = [];
  edicion: boolean;
  fuentesDeRecursosListaArr: any[] = [];
  listaBase: CofinanciacionDocumento[];
  documentoFFIEID = 0;
  constrolRecursos: ControlRecurso[] = [];
  fuentesSeleccionadas: any[]  = [];

  constructor(
    private fb: FormBuilder,
    private commonService: CommonService,
    private cofinanciacionService: CofinanciacionService,
    private activatedRoute: ActivatedRoute,
    public dialog: MatDialog,
    private fuenteFinanciacionService: FuenteFinanciacionService,
    private router: Router
  ) {
    this.maxDate = new Date();
  }

  noGuardado = true;
  ngOnDestroy(): void {
    if (this.addressForm.dirty && this.noGuardado == true) {
      let dialogRef = this.dialog.open(ModalDialogComponent, {
        width: '28em',
        data: { modalTitle: '', modalText: '¿Desea guardar la información registrada?', siNoBoton: true }
      });
      dialogRef.afterClosed().subscribe(result => {
        if (result === true) {
          this.onSubmit();
        }
      });
    }
  }

  openDialog(modalTitle: string, modalText: string, redirect?: boolean) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    if (redirect) {
      dialogRef.afterClosed().subscribe(result => {
        this.router.navigate(['/gestionarFuentes'], {});
      });
    }
  }
  openDialogSiNo(modalTitle: string, modalText: string, borrarForm: any, i: number, j: number, event: number) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton: true }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result === true) {
        if (event == 1) {
          this.removeItemVigencia(borrarForm, i, j);
        } else {
          if (event == 3) {
            if (this.cuentasBancaria(i).value[j].cuentaBancariaId != null) {
              this.fuenteFinanciacionService
                .eliminarCuentaBancaria(this.cuentasBancaria(i).value[j].cuentaBancariaId)
                .subscribe(response => {
                  this.cuentasBancaria(i).removeAt(j);
                  this.openDialog('', '<b>La información a sido eliminada correctamente.</b>', false);
                });
            } else {
              this.cuentasBancaria(i).removeAt(j);
              this.openDialog('', '<b>La información a sido eliminada correctamente.</b>', false);
            }
          } else {
            this.borrarVigencia(borrarForm, i);
          }
        }
      }
    });
  }

  EditMode() {
    this.fuenteRecursosArray.clear();
    const aportante = this.nombresAportantes.find(nom => nom.cofinanciacionAportanteId == this.idAportante);

    if (aportante) {
      this.addressForm.get('tieneRP').setValue(aportante.cuentaConRp == true ? '1' : '0');
    }

    if (this.tipoAportante.FFIE.includes(this.tipoAportanteId.toString())) {
      this.addressForm.get('nombreAportanteFFIE').setValue(aportante);
      this.changeNombreAportanteFFIE();
    } else if (this.tipoAportante.ET.includes(this.tipoAportanteId.toString())) {
      if (aportante.municipioId != null) {
        this.commonService.listaMunicipiosByIdDepartamento(aportante.departamentoId.toString()).subscribe(mun => {
          const valorMunicipio = mun.find(a => a.localizacionId === aportante.municipioId.toString());
          const valorDepartamento = this.departamentos.find(
            a => a.localizacionId == aportante.departamentoId.toString()
          );

          this.municipios = mun.sort((a, b) => {
            let textA = a.descripcion.toUpperCase();
            let textB = b.descripcion.toUpperCase();
            return textA < textB ? -1 : textA > textB ? 1 : 0;
          });
          this.addressForm.get('municipio').setValue(valorMunicipio);
          this.addressForm.get('departamento').setValue(valorDepartamento);
          this.changeMunicipio();
        });
      } else {
        const valorDepartamento = this.departamentos.find(a => a.localizacionId == aportante.departamentoId.toString());
        this.addressForm.get('departamento').setValue(valorDepartamento);
        this.changeDepartamento();
      }

      this.addressForm.get('vigenciaAcuerdo').setValue(aportante);

      this.changeVigencia();
    } else {
      this.addressForm.get('nombreAportante').setValue(aportante.nombreAportanteString);
      this.changeNombreAportanteTercero();
      this.addressForm.get('vigenciaAcuerdo').setValue(aportante);
      this.changeVigencia();
    }

    const listaRegistrosP = this.addressForm.get('registrosPresupuestales') as FormArray;

    this.fuenteFinanciacionService.listaFuenteFinanciacionByAportante(this.idAportante).subscribe(lista => {
      let i = 0;
      lista.forEach(ff => {
        this.fuentesDeRecursosListaArr.push(this.fuentesDeRecursosLista);
        //if (this.tipoAportante.FFIE.includes(this.tipoAportanteId.toString())) {
        //  const tipo = this.tipoDocumentoap.filter(x => x.dominioId == ff.cofinanciacionDocumento.tipoDocumentoId);
        //  this.addressForm.get('tipoDocumento').setValue(tipo[0].dominioId);
        //  const numerodoc = this.listaDocumentosApropiacion.filter(
        //    x => x.cofinanciacionDocumentoId == ff.cofinanciacionDocumentoId
        //  );
        //  this.listaDocumentos = this.listaDocumentosApropiacion.filter(
        //    x => x.tipoDocumentoId == ff.cofinanciacionDocumento.tipoDocumentoId
        //  );
        //  /*this.listaDocumentos = this.listaDocumentosApropiacion.filter(
        //    x => x.cofinanciacionDocumentoId == ff.cofinanciacionDocumentoId
        //  );*/
        //  //this.listaDocumentos.forEach(element => {
        //  //this.valorTotal = numerodoc[0].valorDocumento;
        //  //});
        //  this.valorTotal = 0;
        //  this.listaDocumentos.forEach(element => {
        //    this.valorTotal += element.valorDocumento;
        //  });
        //  this.documentoFFIEID = ff.cofinanciacionDocumentoId;
        //  this.addressForm.get('numerodocumento').setValue(numerodoc[0]);
        //}
        const grupo: FormGroup = this.crearFuenteEdit(ff.valorFuente);
        const fuenteRecursosSeleccionada = this.fuentesDeRecursosListaArr[i].find(
          f => f.codigo === ff.fuenteRecursosCodigo
        );
        i++;
        const listaVigencias = grupo.get('vigencias') as FormArray;
        const listaCuentas = grupo.get('cuentasBancaria') as FormArray;

        grupo.get('cuantasVigencias').setValue(ff.cantVigencias);
        grupo.get('fuenteRecursos').setValue(fuenteRecursosSeleccionada);
        grupo.get('fuenteFinanciacionId').setValue(ff.fuenteFinanciacionId);

        // Vigencias
        let cantidadVigencias = 0;
        ff.vigenciaAporte.forEach(v => {
          const grupoVigencia = this.createVigencia();
          const vigenciaSeleccionada = this.VigenciasAporte.find(vtemp => vtemp == v.tipoVigenciaCodigo);

          grupoVigencia.get('vigenciaAporteId').setValue(v.vigenciaAporteId);
          grupoVigencia.get('vigenciaAportante').setValue(vigenciaSeleccionada);
          grupoVigencia.get('valorVigencia').setValue(v.valorAporte);



          listaVigencias.push(grupoVigencia);
          cantidadVigencias++;
        });

        grupo.get('cuantasVigencias').setValue(cantidadVigencias);

        // Cuentas bancarias
        ff.cuentaBancaria.forEach(ba => {
          const grupoCuenta = this.createCuentaBancaria();
          const bancoSeleccionado: Dominio = this.bancos.find(b => b.codigo === ba.bancoCodigo);

          grupoCuenta.get('cuentaBancariaId').setValue(ba.cuentaBancariaId);
          grupoCuenta.get('numeroCuenta').setValue(ba.numeroCuentaBanco);
          grupoCuenta.get('nombreCuenta').setValue(ba.nombreCuentaBanco);
          grupoCuenta.get('codigoSIFI').setValue(ba.codigoSifi);
          grupoCuenta.get('tipoCuenta').setValue(ba.tipoCuentaCodigo);
          grupoCuenta.get('banco').setValue(bancoSeleccionado);
          grupoCuenta.get('extra').setValue(ba.exenta != null ? ba.exenta.toString() : '');

          listaCuentas.push(grupoCuenta);
        });
        if(ff.controlRecurso){
          ff.controlRecurso.forEach(c => {
            this.constrolRecursos.push(c);
          })
        }

        this.fuenteRecursosArray.push(grupo);
      }); //subscribe
      // Registro Presupuestal
      lista[0].aportante.registroPresupuestal.forEach(rp => {
        const grupoRegistroP = this.createRP();

        grupoRegistroP.get('registroPresupuestalId').setValue(rp.registroPresupuestalId);
        grupoRegistroP.get('numeroRP').setValue(rp.numeroRp);
        grupoRegistroP.get('valorRP').setValue(rp.valorRp);
        grupoRegistroP.get('fecha').setValue(rp.fechaRp);
        let documentose = this.listaDocumentos.filter(x => x.cofinanciacionDocumentoId == rp.cofinanciacionDocumentoId);
        grupoRegistroP.get('numerodocumentoRP').setValue(documentose[0]);

        this.addressForm.get('cuantosRP').setValue(lista[0].aportante.registroPresupuestal.length);

        listaRegistrosP.push(grupoRegistroP);
      });
    });
  }

  crearFuenteEdit(pValorFuenteRecursos: number): FormGroup {
    return this.fb.group({
      fuenteRecursos: [null, Validators.required],
      valorFuenteRecursos: [
        pValorFuenteRecursos,
        Validators.compose([Validators.required, Validators.minLength(2), Validators.maxLength(2)])
      ],
      cuantasVigencias: [1],
      vigencias: this.fb.array([]),
      fuenteFinanciacionId: [null, Validators.required],
      cuentasBancaria: this.fb.array([]),
      tieneRP: [null, Validators.required],
      cuantosRP: [null, Validators.compose([Validators.minLength(1), Validators.maxLength(50)])]
    });
  }

  changeMunicipio() {
    const idMunicipio = this.addressForm.get('municipio').value.localizacionId;
    if(idMunicipio === undefined){
      this.nombresAportantes2=this.nombresAportantes.filter(z=>z.municipioId === undefined);
    }else{
      this.nombresAportantes2=this.nombresAportantes.filter(z=>z.municipioId == (idMunicipio === undefined ? 0 : idMunicipio));
    }
    if(this.nombresAportantes2.length==0)
    {
      this.openDialog("","<b>No tiene acuerdos disponibles.</b>");
    }
  }

  changeVigencia() {
    const vigencia = this.addressForm.get('vigenciaAcuerdo').value;
    this.idAportante = vigencia.cofinanciacionAportanteId;
    this.cargarDocumentos();
  }

  ngOnInit(): void {
    this.VigenciasAporte = this.commonService.vigenciasDesde2015();
    this.activatedRoute.params.subscribe(param => {
      this.tipoAportanteId = param.idTipoAportante;
      this.idAportante = param.idAportante;
      forkJoin([
        this.commonService.listaNombreAportante(),
        this.commonService.listaFuenteTipoFinanciacion(),
        this.commonService.listaBancos(),
        this.commonService.listaDepartamentos(),
        this.cofinanciacionService.listaAportantesByTipoAportante(this.tipoAportanteId)
        //this.fuenteFinanciacionService.listaFuenteFinanciacion(),
      ]).subscribe(res => {
        this.nombresAportantes = res[4].filter(x => x.cofinanciacion.registroCompleto == true); //solo muestro los completos
        if (this.idAportante > 0) {
          //funciona porque recien empezo
          this.edicion = true;
          this.addressForm.markAllAsTouched();
        } else {
          this.edicion = false;
          this.nombresAportantes = this.nombresAportantes.filter(x => x.fuenteFinanciacion.length == 0);
          this.fuentesDeRecursosListaArr.push(res[1]);
        }
        const nombresAportantesTemp: Dominio[] = res[0];

        this.nombresAportantes.forEach(nom => {
          if (!this.solonombres.includes(nom.nombreAportanteString)) {
            this.solonombres.push(nom.nombreAportanteString);
          }
        });

        this.fuentesDeRecursosLista = res[1];

        this.bancos = res[2];
        this.departamentos = res[3].sort((a, b) => {
          let textA = a.descripcion.toUpperCase();
          let textB = b.descripcion.toUpperCase();
          return textA < textB ? -1 : textA > textB ? 1 : 0;
        });
        //this.listaFuentes = res[5];

        if (this.idAportante > 0) {
          this.edicion = true;
          this.addressForm.markAllAsTouched();
          this.EditMode();
        }
      });
    });

    this.addressForm = this.fb.group({
      nombreAportante: [null, Validators.required],
      nombreAportanteFFIE: [null, Validators.required],
      documentoApropiacion: [null, Validators.required],
      tipoDocumento: [null, Validators.required],
      numerodocumento: [null, Validators.compose([Validators.minLength(10), Validators.maxLength(10)])],
      vigenciaAcuerdo: [null, Validators.required],
      departamento: [null, Validators.required],
      municipio: [null, Validators.required],
      tieneRP: [null, Validators.required],
      cuantosRP: [null, Validators.compose([Validators.minLength(1), Validators.maxLength(50)])],
      registrosPresupuestales: this.fb.array([]),
      fuenteRecursosArray: this.fb.array([])
    });

    if(this.idAportante !== 0){
      this.addressForm.controls['cuantosRP'].disable();
    }

    const fuentes = this.addressForm.get('fuenteRecursosArray') as FormArray;
    fuentes.push(this.crearFuente());
  }

  createRP() {
    return this.fb.group({
      registroPresupuestalId: [null, Validators.required],
      numeroRP: [null, Validators.required],
      valorRP: [null, Validators.required],
      fecha: [null, Validators.required],
      numerodocumentoRP: [null, Validators.required]
    });
  }

  onAddRP(){
    const FormNumRP = this.addressForm.get('cuantosRP').value;
    this.addressForm.get('cuantosRP').setValue(+FormNumRP + 1)
    this.registrosPresupuestales.push(this.createRP());
  }

  eliminarRp(numeroRp: string, indexRp: number){
    if(this.constrolRecursos.some(x => x.registroPresupuestal && x.registroPresupuestal.numeroRp === numeroRp)){
      this.openDialog('Validacion', 'No es posible eliminar un Rp con una consignación asociada ');
    }else{
      const rp: RegistroPresupuestal = this.registrosPresupuestales.controls[indexRp].value;
      if(rp.registroPresupuestalId){
      this.listaRpEliminados.push(rp.registroPresupuestalId)
      }
      this.registrosPresupuestales.removeAt(indexRp);
      const cuantosRP = this.addressForm.get('cuantosRP');
      cuantosRP.setValue(Number(cuantosRP.value) - 1);
    }
  }

  CambioNumeroRP() {
    const FormNumRP = this.addressForm.get('cuantosRP').value;
    if (+FormNumRP > this.registrosPresupuestales.length && FormNumRP < 100) {
      while (this.registrosPresupuestales.length < FormNumRP) {
        this.registrosPresupuestales.push(this.createRP());
      }
    } else if (FormNumRP <= this.registrosPresupuestales.length && FormNumRP >= 0) {
      let i = this.registrosPresupuestales.length;
      while (this.registrosPresupuestales.length > FormNumRP) {
        this.registrosPresupuestales.removeAt(i);
        i--;
      }
    }
  }

  changeDepartamento() {
    const idDepartamento = this.addressForm.get('departamento').value.localizacionId;
    this.nombresAportantes2 = this.nombresAportantes.filter(
      z => z.departamentoId == idDepartamento && z.municipioId == null
    );
    this.commonService.listaMunicipiosByIdDepartamento(idDepartamento).subscribe(mun => {
      this.municipios = mun.sort((a, b) => {
        let textA = a.descripcion.toUpperCase();
        let textB = b.descripcion.toUpperCase();
        return textA < textB ? -1 : textA > textB ? 1 : 0;
      });
    });
  }

  cargarDocumentos() {
    this.listaDocumentos = [];
    this.valorTotal = 0;

    this.cofinanciacionService.getDocumentoApropiacionByAportante(this.idAportante).subscribe(listDoc => {
      if (listDoc.length === 0) {
        this.openDialog('Validacion', 'No tiene documentos de apropiacion');
      }

      listDoc.forEach(doc => {
        const cofinanciacionDocumento: CofinanciacionDocumento = {
          cofinanciacionAportanteId: doc.cofinanciacionAportanteId,
          cofinanciacionDocumentoId: doc.cofinanciacionDocumentoId,
          fechaAcuerdo: doc.fechaAcuerdo,
          numeroActa: doc.numeroActa,
          numeroAcuerdo: doc.numeroAcuerdo,
          tipoDocumentoId: doc.tipoDocumentoId,
          valorDocumento: doc.valorDocumento,
          valorTotalAportante: doc.valorTotalAportante,
          vigenciaAporte: doc.vigenciaAporte
        };

        cofinanciacionDocumento.tipoDocumento = {
          activo: doc.tipoDocumento.activo,
          dominioId: doc.tipoDocumento.dominioId,
          nombre: doc.tipoDocumento.nombre,
          tipoDominioId: doc.tipoDocumento.tipoDominioId,
          codigo: doc.tipoDocumento.codigo
        };

        this.valorTotal = this.valorTotal + cofinanciacionDocumento.valorDocumento;
        this.listaDocumentos.push(cofinanciacionDocumento);
      });
    });
  }

  changeNombreAportanteTercero() {
    if (this.addressForm.get('nombreAportante').value) {
      this.nombresAportantes2 = this.nombresAportantes.filter(
        z => z.nombreAportanteString == this.addressForm.get('nombreAportante').value
      );
      if (this.nombresAportantes2.length == 0) {
        this.openDialog('', '<b>No tiene acuerdos disponibles.</b>');
      }
    }
  }

  changeNombreAportante() {
    if (this.addressForm.get('nombreAportante').value) {
      this.idAportante = this.addressForm.get('nombreAportante').value.cofinanciacionAportanteId
        ? this.addressForm.get('nombreAportante').value.cofinanciacionAportanteId
        : this.addressForm.get('nombreAportanteFFIE').value.cofinanciacionAportanteId;

      // tercero
      if (this.tipoAportante.Tercero.includes(this.tipoAportanteId.toString())) {
        const vigenciaSeleccionada = this.addressForm.get('nombreAportante').value.cofinanciacion
          .vigenciaCofinanciacionId;
        const vigenciaRegistro = this.VigenciasAporte.find(vtemp => vtemp == vigenciaSeleccionada);
        this.addressForm.get('vigenciaAcuerdo').setValue(vigenciaRegistro);

        this.cargarDocumentos();
      }

      // FFIE
      if (this.tipoAportante.FFIE.includes(this.tipoAportanteId.toString())) {
        this.cofinanciacionService.getDocumentoApropiacionByAportante(this.idAportante).subscribe(listDoc => {
          if (listDoc.length > 0) {
            this.addressForm.get('numerodocumento').setValue(listDoc[0].numeroActa);
            this.addressForm.get('documentoApropiacion').setValue(listDoc[0].tipoDocumento.nombre);
          } else {
            this.openDialog('Validacion', 'No tiene documentos de apropiacion');
          }
        });
      }

      // ET
      if (this.tipoAportante.ET.includes(this.tipoAportanteId.toString())) {
        //const vigenciaSeleccionada = this.addressForm.get('nombreAportante').value.cofinanciacion.vigenciaCofinanciacionId;
        //const idMunicipio = this.addressForm.get('nombreAportante').value.municipioId;
        //const vigenciaRegistro = this.VigenciasAporte.find(vtemp => vtemp === vigenciaSeleccionada);
        //this.addressForm.get('vigenciaAcuerdo').setValue(vigenciaRegistro);
        this.cargarDocumentos();
      }
    }
  }

  changeNombreAportanteFFIE() {
    this.valorTotal = 0;
    this.addressForm.get('tipoDocumento').setValue(null);

    if (this.addressForm.get('nombreAportanteFFIE').value) {
      this.tipoDocumentoap = [];
      this.idAportante = this.addressForm.get('nombreAportanteFFIE').value.cofinanciacionAportanteId;

      // FFIE
      if (this.tipoAportante.FFIE.includes(this.tipoAportanteId.toString())) {
        const vigencia = this.addressForm.get('nombreAportanteFFIE').value;
        this.idAportante = vigencia.cofinanciacionAportanteId;
        this.cargarDocumentos();
        /*this.cofinanciacionService.getDocumentoApropiacionByAportante(this.idAportante).subscribe(listDoc => {
          if (listDoc.length > 0) {
            //this.addressForm.get('numerodocumento').setValue(listDoc[0].numeroActa);
            //this.addressForm.get('documentoApropiacion').setValue(listDoc[0].tipoDocumento.nombre);
            this.listaDocumentosApropiacion = listDoc;
            this.listaBase = this.listaDocumentosApropiacion;
            listDoc.forEach(element => {
              let m = this.tipoDocumentoap.some(function (item) {
                return item.dominioId === element.tipoDocumentoId;
              });
              if (m) {
                // console.log("ya lo tiene");
              } else {
                this.tipoDocumentoap.push({ dominioId: element.tipoDocumentoId, nombre: element.tipoDocumento.nombre });
              }
            });
          } else {
            this.openDialog('Validacion', 'No tiene documentos de apropiacion');
          }
        });*/
      }
    }
  }

  get fuenteRecursosArray() {
    return this.addressForm.get('fuenteRecursosArray') as FormArray;
  }
  vigencias1(i: number) {
    const control = this.addressForm.get('fuenteRecursosArray') as FormArray;
    return control.controls[i].get('vigencias') as FormArray;
  }
  cuentasBancaria(i: number) {
    const control = this.addressForm.get('fuenteRecursosArray') as FormArray;
    return control.controls[i].get('cuentasBancaria') as FormArray;
  }

  get registrosPresupuestales() {
    return this.addressForm.get('registrosPresupuestales') as FormArray;
  }

  CambioNumerovigencia(j: number) {
    const FormNumvigencias = this.addressForm.value.fuenteRecursosArray[j];

    if(FormNumvigencias.cuantasVigencias!=null && FormNumvigencias.cuantasVigencias!="")
    {
      if (FormNumvigencias.cuantasVigencias > this.vigencias1(j).length && FormNumvigencias.cuantasVigencias < 100) {
        if (FormNumvigencias.cuantasVigencias == 1) {
          this.vigencias1(j).push(
            this.fb.group({
              vigenciaAporteId: [null, Validators.required],
              vigenciaAportante: [null, Validators.required],
              valorVigencia: [
                this.addressForm.get('fuenteRecursosArray')['controls'][j].value.valorFuenteRecursos,
                Validators.compose([Validators.minLength(10), Validators.maxLength(10)])
              ]
            })
          );
          if (this.edicion) this.addressForm.markAllAsTouched();
        } else {
          while (this.vigencias1(j).length < FormNumvigencias.cuantasVigencias) {
            this.vigencias1(j).push(this.createVigencia());
          }
        }
      } else if (
        FormNumvigencias.cuantasVigencias <= this.vigencias1(j).length &&
        FormNumvigencias.cuantasVigencias >= 0
      ) {
        //valido si tiene data
        let bitestavacio = true;
        FormNumvigencias.vigencias.forEach(element => {
          if (element.valorVigencia != null || element.vigenciaAportante != null || element.vigenciaAporteId != null) {
            bitestavacio = false;
          }
        });
        if (bitestavacio) {
          let cuantas = FormNumvigencias.cuantasVigencias;
          var resta = this.vigencias1(j).length - FormNumvigencias.cuantasVigencias;
          for (let secuenciaa = this.vigencias1(j).length; secuenciaa >= cuantas; secuenciaa--) {
            //this.removeItemVigencia(this.vigencias1(j), this.vigencias1(j).length - 1,j,false);
            this.vigencias1(j).removeAt(secuenciaa);
          }
          this.addressForm
            .get('fuenteRecursosArray')
            ['controls'][j].get('cuantasVigencias')
            .setValue(this.vigencias1(j).length);
        } else {
          this.openDialog(
            '',
            '<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>'
          );
          this.addressForm
            .get('fuenteRecursosArray')
            ['controls'][j].get('cuantasVigencias')
            .setValue(this.vigencias1(j).length);
        }
      }
    }
  }

  agregarCuentaBancaria(i) {
    let listaFuentes = this.addressForm.get('fuenteRecursosArray') as FormArray;
    let listabancos = listaFuentes.controls[i].get('cuentasBancaria') as FormArray;

    this.cuentasBancaria(i).push(this.createCuentaBancaria());
  }

  agregaFuente() {
    if (this.addressForm.value.fuenteRecursosArray.length == this.fuentesDeRecursosLista.length) {
      this.openDialog('', '<b>Ya cuenta con los tipos de fuente de financiación disponibles.</b>');
      return;
    }
    this.addressForm.value.fuenteRecursosArray.forEach(element => {
      this.fuentesDeRecursosListaArr.push(this.fuentesDeRecursosLista.filter(x => x != element.fuenteRecursos));
    });
    this.fuenteRecursosArray.push(this.crearFuente());
  }

  createCuentaBancaria(): FormGroup {
    return this.fb.group({
      cuentaBancariaId: [null, Validators.required],
      numeroCuenta: [
        null,
        Validators.compose([Validators.required, Validators.minLength(1), Validators.maxLength(50)])
      ],
      nombreCuenta: [
        null,
        Validators.compose([Validators.required, Validators.minLength(1), Validators.maxLength(100)])
      ],
      codigoSIFI: [null, Validators.compose([Validators.required, Validators.minLength(1), Validators.maxLength(6)])],
      tipoCuenta: [null, Validators.required],
      banco: [null, Validators.required],
      extra: [null, Validators.required]
    });
  }

  crearFuente(): FormGroup {
    return this.fb.group({
      fuenteRecursos: [null, Validators.required],
      valorFuenteRecursos: [
        null,
        Validators.compose([Validators.required, Validators.minLength(2), Validators.maxLength(2)])
      ],
      cuantasVigencias: [null, Validators.required],
      vigencias: this.fb.array([]),
      fuenteFinanciacionId: [null, Validators.required],
      departamento: [null, Validators.required],
      municipio: [null, Validators.required],
      tieneRP: [null, Validators.required],
      cuantosRP: [null, Validators.compose([Validators.minLength(1), Validators.maxLength(50)])],
      cuentasBancaria: this.fb.array([
        this.fb.group({
          cuentaBancariaId: [null, Validators.required],
          numeroCuenta: [
            null,
            Validators.compose([Validators.required, Validators.minLength(1), Validators.maxLength(50)])
          ],
          nombreCuenta: [
            null,
            Validators.compose([Validators.required, Validators.minLength(1), Validators.maxLength(100)])
          ],
          codigoSIFI: [
            null,
            Validators.compose([Validators.required, Validators.minLength(1), Validators.maxLength(6)])
          ],
          tipoCuenta: [null, Validators.required],
          banco: [null, Validators.required],
          extra: [null, Validators.required]
        })
      ])
    });
  }

  createVigencia(): FormGroup {
    return this.fb.group({
      vigenciaAporteId: [null, Validators.required],
      vigenciaAportante: [null, Validators.required],
      valorVigencia: [null, Validators.compose([Validators.minLength(10), Validators.maxLength(10)])]
    });
  }

  borrarArray(borrarForm: any, i: number, tipo: number, posicion: number) {
    this.openDialogSiNo('', '<b>¿Está seguro de eliminar este registro?</b>', borrarForm, i, posicion, tipo);
  }

  borrarVigencia(borrarForm: any, i: number) {
    // console.log( borrarForm.value[i] );

    // if (borrarForm.value[i].valorFuenteRecursos && borrarForm.value[i].valorFuenteRecursos > 0)
    // {
    //   this.openDialog("","<b>debe distribuir.</b>",false);
    //   return false;
    // }

    if (borrarForm.value[i].fuenteFinanciacionId != null) {
      // cuando solo hay una fuente
      if (this.addressForm.value.fuenteRecursosArray.length == 1) {
        this.fuenteFinanciacionService
          .EliminarFuentesFinanciacionCompleto(borrarForm.value[i].fuenteFinanciacionId)
          .subscribe(response => {
            this.openDialog('', response.message, false);
            if (response.code === '200') {
              borrarForm.removeAt(i);
              this.router.navigate(['/gestionarFuentes']);
            }
          });
      } else {
        this.fuenteFinanciacionService
          .validarEliminarFuentesFinanciacion(borrarForm.value[i].fuenteFinanciacionId)
          .subscribe(response => {
            this.openDialog('', response.message, false);
            if (!response.isValidation) {
              console.log( borrarForm.controls[i]  )
              this.listaFuentesEliminadas.push( borrarForm.controls[i] );
              borrarForm.removeAt(i);
              this.openDialog('', '<b>La información ha sido eliminada correctamente.</b>', false);
            }else{

            }
          });
      }
    } else {
      borrarForm.removeAt(i);
      this.openDialog('', '<b>La información ha sido eliminada correctamente.</b>', false);
    }
  }

  borrarArrayVigencias(borrarForm: any, i: number, j: number) {
    this.openDialogSiNo("", "<b>¿Está seguro de eliminar este registro?</b>", borrarForm, i, j, 1);
  }

  removeItemVigencia(borrarForm: FormArray, i: number,j:number,mensaje=true)
  {
    if(borrarForm.value[i].vigenciaAporteId!=null)
    {
      //this.cofinanciacionService.eliminarVigencia(borrarForm.value[i].vigenciaAporteId).subscribe(response=>
        //{
          this.listaVigenciasEliminadas.push(borrarForm.controls[i]);
          //borrarForm.controls[i].get('eliminado').setValue( true );

          //console.log(borrarForm.controls[i].get('eliminado').value)
          borrarForm.removeAt(i);
          this.addressForm.get("fuenteRecursosArray")['controls'][j].get("cuantasVigencias").setValue(this.vigencias1(j).length);

          this.openDialog("","<b>La información a sido eliminada correctamente.</b>",false);
        //});

    }
    else
    {
      borrarForm.removeAt(i);
      this.addressForm
        .get('fuenteRecursosArray')
        ['controls'][j].get('cuantasVigencias')
        .setValue(this.vigencias1(j).length);
      if (mensaje) {
        this.openDialog('', '<b>La información a sido eliminada correctamente.</b>', false);
      }
    }
  }
  // evalua tecla a tecla
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  filterDocumento(variable) {
    this.valorTotal = 0;
    this.listaDocumentosApropiacion = this.listaBase.filter(x => x.tipoDocumentoId == variable);
    this.listaDocumentos = this.listaDocumentosApropiacion;
    console.log(this.listaDocumentos);
    this.documentoFFIEID = this.listaDocumentos[0].cofinanciacionDocumentoId;
    if (this.tipoAportante.FFIE.includes(this.tipoAportanteId.toString())) {
      this.listaDocumentos.forEach(element => {
        this.valorTotal += element.valorDocumento;
      });
      //this.valorTotal += this.listaDocumentos[0].valorDocumento;
    } else {
      this.listaDocumentos.forEach(element => {
        this.valorTotal += element.valorDocumento;
      });
    }
  }

  changeVigenciaFfie() {
    const vigencia = this.addressForm.get('nombreAportanteFFIE').value;
    this.idAportante = vigencia.cofinanciacionAportanteId;
    this.cargarDocumentos();
  }

  onSubmit() {
    this.edicion = true;
    this.addressForm.markAllAsTouched();
    //no puedo validarlo, porque puede ser parcial
    //if (this.addressForm.valid) {

    let bitValorok = true;
    const lista: FuenteFinanciacion[] = [];
    const listaRP: RegistroPresupuestal[] = [];

    let usuario = '';
    if (localStorage.getItem('actualUser')) {
      usuario = localStorage.getItem('actualUser');
      usuario = JSON.parse(usuario).email;
    }
    let valortotla = 0;
    let valorBase = this.valorTotal;
    let valorBase2 = 0;

    this.listaFuentesEliminadas.forEach( fu => {
      const fuente: FuenteFinanciacion = {
        fuenteFinanciacionId: fu.get('fuenteFinanciacionId').value,
        aportanteId: this.idAportante,
        fuenteRecursosCodigo: fu.get('fuenteRecursos').value?.codigo,
        valorFuente: fu.get('valorFuenteRecursos').value,
        cantVigencias: fu.get('cuantasVigencias').value,
        cuentaBancaria: [],
        vigenciaAporte: [],
        cofinanciacionDocumentoId: this.documentoFFIEID,
        eliminado: true,
      }
      lista.push(fuente);
    });

    this.fuenteRecursosArray.controls.forEach(controlFR => {
      const vigencias = controlFR.get('vigencias') as FormArray;
      if (vigencias.controls.length == 0) {
        valortotla += controlFR.get('valorFuenteRecursos').value;
      }
      const fuente: FuenteFinanciacion = {
        fuenteFinanciacionId: controlFR.get('fuenteFinanciacionId').value,
        aportanteId: this.idAportante,
        fuenteRecursosCodigo: controlFR.get('fuenteRecursos').value?.codigo,
        valorFuente: controlFR.get('valorFuenteRecursos').value,
        cantVigencias: controlFR.get('cuantasVigencias').value,
        cuentaBancaria: [],
        vigenciaAporte: [],
        cofinanciacionDocumentoId: this.documentoFFIEID,
        eliminado: false,
        aportante: {
          cofinanciacionAportanteId: this.idAportante,
          cuentaConRp: this.addressForm.get('tieneRP').value == '1' ? true : false,
          cofinanciacionId: null,
          tipoAportanteId: null,
          municipioId: null,
          departamentoId: null,
          cofinanciacionDocumento: null
        }
      };

      this.listaVigenciasEliminadas.forEach( vi => {
        console.log( vi )
        const vigenciaAporte: VigenciaAporte = {
               vigenciaAporteId: vi.get('vigenciaAporteId').value,
               fuenteFinanciacionId: 0,
               tipoVigenciaCodigo: vi.get('vigenciaAportante').value,
               valorAporte: vi.get('valorVigencia').value,
               eliminado: true

             };
             fuente.vigenciaAporte.push(vigenciaAporte);
      });

      if (vigencias.controls.length > 0) {
        let totalaportes = 0;
        vigencias.controls.forEach(controlVi => {
          const vigenciaAporte: VigenciaAporte = {
            vigenciaAporteId: controlVi.get('vigenciaAporteId').value,
            fuenteFinanciacionId: controlFR.get('fuenteFinanciacionId').value,
            tipoVigenciaCodigo: controlVi.get('vigenciaAportante').value,
            valorAporte: controlVi.get('valorVigencia').value,
            eliminado: false
          };
          totalaportes += controlVi.get('valorVigencia').value;
          valortotla += controlVi.get('valorVigencia').value ? controlVi.get('valorVigencia').value : 0;
          fuente.vigenciaAporte.push(vigenciaAporte);
        });
        //si tengo vigencias mi valor base es la fuente
        valorBase2 += controlFR.get('valorFuenteRecursos').value;
      }

      const cuentas = controlFR.get('cuentasBancaria') as FormArray;
      cuentas.controls.forEach(controlBa => {
        const cuentaBancaria: CuentaBancaria = {
          cuentaBancariaId: controlBa.get('cuentaBancariaId').value,
          bancoCodigo: controlBa.get('banco').value?.codigo,
          codigoSifi: controlBa.get('codigoSIFI').value,
          exenta: controlBa.get('extra').value,
          fuenteFinanciacionId: controlFR.get('fuenteFinanciacionId').value,
          nombreCuentaBanco: controlBa.get('nombreCuenta').value,
          numeroCuentaBanco: controlBa.get('numeroCuenta').value,
          tipoCuentaCodigo: controlBa.get('tipoCuenta').value
        };

        fuente.cuentaBancaria.push(cuentaBancaria);
      });

      lista.push(fuente);
    });

    // cargo los RP en la lista para guardar
    const registrosPresupuestales = this.addressForm.get('registrosPresupuestales') as FormArray;
    if (registrosPresupuestales) {
      registrosPresupuestales.controls.forEach(controlRP => {
        const registroPresupuestal: RegistroPresupuestal = {
          aportanteId: this.idAportante,
          fechaRp: controlRP.get('fecha').value,
          numeroRp: controlRP.get('numeroRP').value,
          valorRp: controlRP.get('valorRP').value,
          registroPresupuestalId: controlRP.get('registroPresupuestalId').value,
          cofinanciacionDocumentoId: controlRP.get('numerodocumentoRP').value.cofinanciacionDocumentoId
        };
        listaRP.push(registroPresupuestal);
      });
    }

    //si tengo vigencias

    //console.log(valorBase2, )

    if (valorBase2 > 0) {

      console.log( valorBase2, valorBase, valortotla )

      if(valorBase2>0)
      {
        if(valorBase2!=valortotla)
        {
          this.openDialog("","<b>Los valores de aporte de las vigencias son diferentes al valor de aporte de la fuente.</b>");
          bitValorok=false;
          return false;
        }
        //aunque tenga vigencias, valido sobre el documento para ffie
        if(this.tipoAportante.FFIE.includes(this.tipoAportanteId.toString()))
        {
          if(valorBase!=valortotla)
          {
           this.openDialog("","<b>Los valores de aporte de las vigencias son diferentes al valor de aporte de la fuente.</b>");
            bitValorok=false;
            return false;
          }
        }
      }
      //aunque tenga vigencias, valido sobre el documento para ffie
      if (this.tipoAportante.FFIE.includes(this.tipoAportanteId.toString())) {
        if (valorBase != valortotla) {
          this.openDialog(
            '',
            '<b>Los valores de aporte de las vigencias son diferentes al valor de aporte de la fuente.</b>'
          );
          bitValorok = false;
          return false;
        }
      }
    } else {
      if (valorBase != valortotla) {
        this.openDialog(
          '',
          '<b>Los valores de aporte de las vigencias son diferentes al valor de aporte de la fuente.</b>'
        );
        bitValorok = false;
        return false;
      }
    }
    let valorTotalRps = 0;
    this.registrosPresupuestales.value.forEach(element => {
      valorTotalRps = valorTotalRps + element.valorRP
    });

    if(!this.tipoAportante.FFIE.includes(this.tipoAportanteId.toString()) && valorTotalRps > valortotla ){
      this.openDialog(
        '',
        '<b>Los valores RP son mayores que el valor total del aporte de la fuente</b>'
      );
      return false;
    }


    if (bitValorok) {
      // guardo primero los RP
      forkJoin([
        from(listaRP).pipe(
          mergeMap(cb => this.fuenteFinanciacionService.createEditBudgetRecords(cb).pipe(tap())),
          toArray()
        )
      ]).subscribe(() => {
        // despues guardo las fuentes
        forkJoin([
          from(lista).pipe(
            mergeMap(ff => this.fuenteFinanciacionService.createEditFuentesFinanciacion(ff).pipe(tap())),
            toArray()
          )
        ]).subscribe(respuesta => {
          const res = respuesta[0][0] as Respuesta;
          if (res.code === '200') {
            this.openDialog('', `<b>${res.message}</b>`, true);
            this.noGuardado = false;
          }
        });
      });
      if(this.listaRpEliminados.length > 0){
      this.fuenteFinanciacionService.deleteBudgetRecords(this.listaRpEliminados).subscribe()
      }
      this.data = lista;
    }

    //}
  }

  validateonevige(j) {
    const FormNumvigencias = this.addressForm.value.fuenteRecursosArray[j];
    if (FormNumvigencias.cuantasVigencias == 1) {
      this.vigencias1(j).controls[0].get('valorVigencia').setValue(FormNumvigencias.valorFuenteRecursos);
    }
  }
  changefuenteRecursos(j) {
    let fuentes = [];
    this.addressForm.value.fuenteRecursosArray.forEach(element => {
      if (fuentes.includes(element.fuenteRecursos)) {
        this.openDialog('', '<b>No puedes tener dos tipos de fuentes iguales por aportante</b>');
        this.addressForm.value.fuenteRecursosArray[j].fuenteRecursos = null;
      } else {
        fuentes.push(element.fuenteRecursos);
      }
    });
  }
  mostrarDocumento() {
    var documento = this.addressForm.get('numerodocumento').value;

    this.valorTotal = documento.valorDocumento;
    this.listaDocumentos.push(documento);
  }

  sinRPS() {
    this.registrosPresupuestales.clear();
    this.addressForm.get('cuantosRP').setValue(0);
  }

  validarValorRpXDocumentoApropiacion(event: any, i: number){
    this.valorRPTmp = 0;
    this.valorDocumentoTmp = 0;

    let valorTotalRP = 0;

    let listaDocumentoSeleccionada = this.listaDocumentos.find(r => r.cofinanciacionAportanteId == event.value.cofinanciacionAportanteId && r.cofinanciacionDocumentoId == event.value.cofinanciacionDocumentoId);
    this.valorDocumentoTmp = listaDocumentoSeleccionada.valorDocumento;
    this.valorRPTmp = this.registrosPresupuestales.controls[i].get('valorRP').value;

    this.registrosPresupuestales.controls.forEach(element => {

      if (element.value.numerodocumentoRP.numeroAcuerdo === event.value.numeroAcuerdo) {
        valorTotalRP += element.value.valorRP;;

        if (valorTotalRP > this.valorDocumentoTmp) {
          this.registrosPresupuestales.controls[i].get('valorRP').setValue(null);
          this.registrosPresupuestales.controls[i].get('numerodocumentoRP').setValue(null);
          this.openDialog(
            '',
            '<b>Los valores RP son mayores que el valor del documento de apropiación</b>'
          );
          return;
        }

      }

    });

    if(this.valorRPTmp > this.valorDocumentoTmp){
      this.registrosPresupuestales.controls[i].get('valorRP').setValue(null);
      this.registrosPresupuestales.controls[i].get('numerodocumentoRP').setValue(null);
      this.openDialog(
        '',
        '<b>Los valores RP son mayores que el valor del documento de apropiación</b>'
      );
      return;
    }
  }

  selectChangeFuenteRecursos(event, index) {
    const codigo = event.value.codigo;
    this.fuentesSeleccionadas[index] = codigo;
    let fuentesRecursos;
    if (index > 0) fuentesRecursos = [...this.fuentesDeRecursosListaArr[index - 1]];
    else fuentesRecursos = [...this.fuentesDeRecursosLista];

    for (let i = 0; i < this.fuentesSeleccionadas.length; i++) {
      const element = this.fuentesSeleccionadas[i];
      for (let j = 0; j < fuentesRecursos.length; j++) {
        const element2 = fuentesRecursos[j];
        if (element2.codigo === element) fuentesRecursos.splice(j, 1);
      }
    }

    for (let i = 0; i < this.fuentesDeRecursosListaArr.length; i++) {
      if (i > index) this.fuentesDeRecursosListaArr[i] = fuentesRecursos;
    }
  }
}
