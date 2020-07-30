import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray, ControlValueAccessor, FormGroup, FormControl } from '@angular/forms';
import { CommonService, Dominio, Localizacion, TiposAportante } from 'src/app/core/_services/common/common.service';
import { CofinanciacionService, CofinanciacionAportante, CofinanciacionDocumento } from 'src/app/core/_services/Cofinanciacion/cofinanciacion.service';
import { forkJoin, from } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { FuenteFinanciacionService, FuenteFinanciacion, CuentaBancaria, RegistroPresupuestal, VigenciaAporte } from 'src/app/core/_services/fuenteFinanciacion/fuente-financiacion.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { CuentaBancariaService } from 'src/app/core/_services/cuentaBancaria/cuenta-bancaria.service';
import { mergeMap, tap, toArray } from 'rxjs/operators';
import { Respuesta } from 'src/app/core/_services/autenticacion/autenticacion.service';

@Component({
  selector: 'app-registrar',
  templateUrl: './registrar.component.html',
  styleUrls: ['./registrar.component.scss']
})
export class RegistrarComponent implements OnInit {

  loading = false;

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

  data;

  constructor(private fb: FormBuilder,
              private commonService: CommonService,
              private cofinanciacionService: CofinanciacionService,
              private activatedRoute: ActivatedRoute,
              public dialog: MatDialog,
              private fuenteFinanciacionService: FuenteFinanciacionService
  ) {
    this.maxDate = new Date();
  }


  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  EditMode() {

    this.fuenteRecursosArray.clear();

    const aportante = this.nombresAportantes.find(nom => nom.cofinanciacionAportanteId === this.idAportante);
    this.addressForm.get('nombreAportante').setValue(aportante);
    this.changeNombreAportante();
    const listaRegistrosP = this.addressForm.get('registrosPresupuestales') as FormArray;

    this.fuenteFinanciacionService.listaFuenteFinanciacionByAportante(this.idAportante).subscribe(lista => {
      lista.forEach(ff => {

        const grupo: FormGroup = this.crearFuenteEdit(ff.valorFuente);
        const fuenteRecursosSeleccionada = this.fuentesDeRecursosLista.find(f => f.codigo === ff.fuenteRecursosCodigo);
        const listaVigencias = grupo.get('vigencias') as FormArray;
        const listaCuentas = grupo.get('cuentasBancaria') as FormArray;

        grupo.get('cuantasVigencias').setValue(ff.cantVigencias);
        grupo.get('fuenteRecursos').setValue(fuenteRecursosSeleccionada);
        grupo.get('fuenteFinanciacionId').setValue(ff.fuenteFinanciacionId);

        // Vigencias
        ff.vigenciaAporte.forEach(v => {
          const grupoVigencia = this.createVigencia();
          const vigenciaSeleccionada = this.VigenciasAporte.find(vtemp => vtemp === v.tipoVigenciaCodigo);

          grupoVigencia.get('vigenciaAporteId').setValue(v.vigenciaAporteId);
          grupoVigencia.get('vigenciaAportante').setValue(vigenciaSeleccionada);
          grupoVigencia.get('valorVigencia').setValue(v.valorAporte);

          listaVigencias.push(grupoVigencia);
        });

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
          grupoCuenta.get('extra').setValue(ba.exenta.toString());

          listaCuentas.push(grupoCuenta);
          console.log(listaCuentas);
        });
        // Registro Presupuestal
        ff.aportante.registroPresupuestal.forEach(rp => {
          const grupoRegistroP = this.createRP();

          grupoRegistroP.get('registroPresupuestalId').setValue(rp.registroPresupuestalId);
          grupoRegistroP.get('numeroRP').setValue(rp.numeroRp);
          grupoRegistroP.get('fecha').setValue(rp.fechaRp);

          this.addressForm.get('cuantosRP').setValue(ff.aportante.registroPresupuestal.length);

          listaRegistrosP.push(grupoRegistroP);

        });

        this.fuenteRecursosArray.push(grupo);
      });
    });
  }

  crearFuenteEdit(pValorFuenteRecursos: number): FormGroup {
    return this.fb.group({
      fuenteRecursos: [null, Validators.required],
      valorFuenteRecursos: [pValorFuenteRecursos, Validators.compose([
        Validators.required, Validators.minLength(2), Validators.maxLength(2)])
      ],
      cuantasVigencias: [1],
      vigencias: this.fb.array([]),
      fuenteFinanciacionId: [null],
      cuentasBancaria: this.fb.array([
      ]),
      tieneRP: [null],
      cuantosRP: [null, Validators.compose([
        Validators.minLength(5), Validators.maxLength(50)])]

    });
  }

  ngOnInit(): void {

    this.VigenciasAporte = this.commonService.vigenciasDesde2015();
    this.activatedRoute.params.subscribe(param => {
      this.tipoAportanteId = param.idTipoAportante;
      this.idAportante = param.idAportante;

      // console.log(this.tipoAportanteId, param);

      forkJoin([
        this.commonService.listaNombreAportante(),
        this.commonService.listaFuenteRecursos(),
        this.commonService.listaBancos(),
        this.commonService.listaDepartamentos(),
        this.cofinanciacionService.listaAportantesByTipoAportante(this.tipoAportanteId),
        this.fuenteFinanciacionService.listaFuenteFinanciacion()
      ]).subscribe(res => {

        this.nombresAportantes = res[4];

        const nombresAportantesTemp: Dominio[] = res[0];

        this.nombresAportantes.forEach(nom => {
          const s = nombresAportantesTemp.find(nomTemp => nomTemp.dominioId === nom.nombreAportanteId);
          if (s) {
            nom.nombreAportante = s.nombre;
          }

        });

        this.fuentesDeRecursosLista = res[1];
        this.bancos = res[2];
        this.departamentos = res[3];
        this.listaFuentes = res[5];

        if (this.idAportante > 0) {
          this.EditMode();
        }

      });
    });



    this.addressForm = this.fb.group({
      nombreAportante: [null],
      documentoApropiacion: [null],
      numerodocumento: [null, Validators.compose([
        Validators.minLength(10), Validators.maxLength(10)])
      ],
      vigenciaAcuerdo: [],
      departamento: [],
      municipio: [],
      tieneRP: [null],
      cuantosRP: [null, Validators.compose([
        Validators.minLength(5), Validators.maxLength(50)])]
      , registrosPresupuestales: this.fb.array([])
      , fuenteRecursosArray: this.fb.array([]),
    });

    const fuentes = this.addressForm.get('fuenteRecursosArray') as FormArray;
    fuentes.push(this.crearFuente());
  }

  createRP() {
    return this.fb.group({
      registroPresupuestalId: [],
      numeroRP: [null],
      fecha: [null]
    });
  }

  CambioNumeroRP() {
    const FormNumRP = this.addressForm.get('cuantosRP').value;
    console.log(FormNumRP);
    if (FormNumRP > this.registrosPresupuestales.length && FormNumRP < 100) {
      while (this.registrosPresupuestales.length < FormNumRP) {
        this.registrosPresupuestales.push(this.createRP());
      }
    } else if (FormNumRP <= this.registrosPresupuestales.length && FormNumRP >= 0) {
      while (this.registrosPresupuestales.length > FormNumRP) {
        this.borrarArray(this.registrosPresupuestales, this.registrosPresupuestales.length - 1);
      }
    }
  }

  changeDepartamento() {
    const idDepartamento = this.addressForm.get('departamento').value.localizacionId;
    this.commonService.listaMunicipiosByIdDepartamento(idDepartamento).subscribe(mun => {
      this.municipios = mun;
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
          tipoDocumentoId: doc.tipoDocumentoId,
          valorDocumento: doc.valorDocumento,
          valorTotalAportante: doc.valorTotalAportante,
          vigenciaAporte: doc.vigenciaAporte,
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

  changeNombreAportante() {
    console.log('aqui el cambio');
    if (this.addressForm.get('nombreAportante').value) {

      this.idAportante = this.addressForm.get('nombreAportante').value.cofinanciacionAportanteId;


      // tercero
      if (this.tipoAportante.Tercero.includes(this.tipoAportanteId.toString())) {
        const vigenciaSeleccionada = this.addressForm.get('nombreAportante').value.cofinanciacion.vigenciaCofinanciacionId;
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
          }
          else {
            this.openDialog('Validacion', 'No tiene documentos de apropiacion');
          }
        });
      }

      // ET
      if (this.tipoAportante.ET.includes(this.tipoAportanteId.toString())) {
        const vigenciaSeleccionada = this.addressForm.get('nombreAportante').value.cofinanciacion.vigenciaCofinanciacionId;
        const idMunicipio = this.addressForm.get('nombreAportante').value.municipioId;
        const vigenciaRegistro = this.VigenciasAporte.find(vtemp => vtemp === vigenciaSeleccionada);

        this.addressForm.get('vigenciaAcuerdo').setValue(vigenciaRegistro);

        this.commonService.listaMunicipiosByIdDepartamento(idMunicipio.toString().substring(0, 5)).subscribe(mun => {
          const valorMunicipio = mun.find(a => a.localizacionId === idMunicipio.toString());
          const valorDepartamento = this.departamentos.find(a => a.localizacionId == idMunicipio.toString().substring(0, 5));

          this.municipios = mun;
          this.addressForm.get('municipio').setValue(valorMunicipio);
          this.addressForm.get('departamento').setValue(valorDepartamento);
        });

        this.cargarDocumentos();
      }
    }
  }

  get fuenteRecursosArray() {
    return this.addressForm.get('fuenteRecursosArray') as FormArray;
  }
  get vigencias1() {
    const control = this.addressForm.get('fuenteRecursosArray') as FormArray;
    return control.controls[0].get('vigencias') as FormArray;
  }
  get cuentasBancaria() {
    const control = this.addressForm.get('fuenteRecursosArray') as FormArray;
    return control.controls[0].get('cuentasBancaria') as FormArray;
  }

  get registrosPresupuestales() {
    return this.addressForm.get('registrosPresupuestales') as FormArray;
  }


  CambioNumerovigencia(j: number) {
    const FormNumvigencias = this.addressForm.value.fuenteRecursosArray[j];
    if (FormNumvigencias.cuantasVigencias > this.vigencias1.length && FormNumvigencias.cuantasVigencias < 100) {
      while (this.vigencias1.length < FormNumvigencias.cuantasVigencias) {
        this.vigencias1.push(this.createVigencia());
      }
    } else if (FormNumvigencias.cuantasVigencias <= this.vigencias1.length && FormNumvigencias.cuantasVigencias >= 0) {
      while (this.vigencias1.length > FormNumvigencias.cuantasVigencias) {
        this.borrarArray(this.vigencias1, this.vigencias1.length - 1);
      }
    }
  }

  agregarCuentaBancaria() {
    this.cuentasBancaria.push(this.createCuentaBancaria());
  }

  agregaFuente() {
    this.fuenteRecursosArray.push(this.crearFuente());
  }

  createCuentaBancaria(): FormGroup {
    return this.fb.group({
      cuentaBancariaId: [],
      numeroCuenta: [null, Validators.compose([
        Validators.required, Validators.minLength(5), Validators.maxLength(50)])
      ],
      nombreCuenta: [null, Validators.compose([
        Validators.required, Validators.minLength(5), Validators.maxLength(100)])
      ],
      codigoSIFI: [null, Validators.compose([
        Validators.required, Validators.minLength(6), Validators.maxLength(6)])
      ],
      tipoCuenta: ['free', Validators.required],
      banco: [null, Validators.required],
      extra: ['free', Validators.required]
    });
  }

  crearFuente(): FormGroup {
    return this.fb.group({
      fuenteRecursos: [null, Validators.required],
      valorFuenteRecursos: [null, Validators.compose([
        Validators.required, Validators.minLength(2), Validators.maxLength(2)])
      ],
      cuantasVigencias: [null],
      vigencias: this.fb.array([]),
      fuenteFinanciacionId: [null],
      departamento: [],
      municipio: [],
      tieneRP: [null],
      cuantosRP: [null, Validators.compose([
        Validators.minLength(5), Validators.maxLength(50)])]
      , cuentasBancaria: this.fb.array([
        this.fb.group({
          cuentaBancariaId: [],
          numeroCuenta: [null, Validators.compose([
            Validators.required, Validators.minLength(5), Validators.maxLength(50)])
          ],
          nombreCuenta: [null, Validators.compose([
            Validators.required, Validators.minLength(5), Validators.maxLength(100)])
          ],
          codigoSIFI: [null, Validators.compose([
            Validators.required, Validators.minLength(6), Validators.maxLength(6)])
          ],
          tipoCuenta: ['free', Validators.required],
          banco: [null, Validators.required],
          extra: ['free', Validators.required]
        })
      ]),
    });
  }

  createVigencia(): FormGroup {
    return this.fb.group({
      vigenciaAporteId: [],
      vigenciaAportante: [null],
      valorVigencia: [null, Validators.compose([
        Validators.minLength(10), Validators.maxLength(10)])
      ]
    });
  }

  borrarArray(borrarForm: any, i: number) {
    borrarForm.removeAt(i);
  }

  // evalua tecla a tecla
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  onSubmit() {
    // console.log(this.addressForm);
    if (this.addressForm.valid) {

      const lista: FuenteFinanciacion[] = [];
      const listaRP: RegistroPresupuestal[] = [];

      let usuario = '';
      if (localStorage.getItem('actualUser')) {
        usuario = localStorage.getItem('actualUser');
        usuario = JSON.parse(usuario).email;
      }

      this.fuenteRecursosArray.controls.forEach(controlFR => {
        const fuente: FuenteFinanciacion = {
          fuenteFinanciacionId: controlFR.get('fuenteFinanciacionId').value,
          aportanteId: this.idAportante,
          fuenteRecursosCodigo: controlFR.get('fuenteRecursos').value.codigo,
          valorFuente: controlFR.get('valorFuenteRecursos').value,
          cantVigencias: controlFR.get('cuantasVigencias').value,
          cuentaBancaria: [],
          vigenciaAporte: [],

        };

        const vigencias = controlFR.get('vigencias') as FormArray;

        if (vigencias) {
          vigencias.controls.forEach(controlVi => {
            const vigenciaAporte: VigenciaAporte = {
              vigenciaAporteId: controlVi.get('vigenciaAporteId').value,
              fuenteFinanciacionId: controlFR.get('fuenteFinanciacionId').value,
              tipoVigenciaCodigo: controlVi.get('vigenciaAportante').value,
              valorAporte: controlVi.get('valorVigencia').value,

            };

            fuente.vigenciaAporte.push(vigenciaAporte);

          });
        }

        const cuentas = controlFR.get('cuentasBancaria') as FormArray;
        cuentas.controls.forEach(controlBa => {
          const cuentaBancaria: CuentaBancaria = {
            cuentaBancariaId: controlBa.get('cuentaBancariaId').value,
            bancoCodigo: controlBa.get('banco').value.codigo,
            codigoSifi: controlBa.get('codigoSIFI').value,
            exenta: controlBa.get('extra').value,
            fuenteFinanciacionId: controlFR.get('fuenteFinanciacionId').value,
            nombreCuentaBanco: controlBa.get('nombreCuenta').value,
            numeroCuentaBanco: controlBa.get('numeroCuenta').value,
            tipoCuentaCodigo: controlBa.get('tipoCuenta').value,

          };

          fuente.cuentaBancaria.push(cuentaBancaria);
        });

        const registrosPresupuestales = this.addressForm.get('registrosPresupuestales') as FormArray;
        if (registrosPresupuestales) {
          registrosPresupuestales.controls.forEach(controlRP => {
            const registroPresupuestal: RegistroPresupuestal = {
              aportanteId: this.idAportante,
              fechaRp: controlRP.get('fecha').value,
              numeroRp: controlRP.get('numeroRP').value,
              registroPresupuestalId: controlRP.get('registroPresupuestalId').value,
            };
            listaRP.push(registroPresupuestal);
          });
        }

        lista.push(fuente);
      });

      forkJoin([
        from(lista)
          .pipe(mergeMap(ff => this.fuenteFinanciacionService.createEditFuentesFinanciacion(ff)
            .pipe(
              tap()
            )
          ),
            toArray()),
        from(listaRP)
          .pipe(mergeMap(cb => this.fuenteFinanciacionService.createEditBudgetRecords(cb)
            .pipe(
              tap()
            )
          ),
            toArray()),
      ])
        .subscribe(respuesta => {
          const res = respuesta[0][0] as Respuesta;
          if (res.code === '200') {
            this.openDialog('Fuente Financiación', res.message);
          }
          console.log(respuesta);
        });



      this.data = lista;

    }
  }
}
