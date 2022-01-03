import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormArray } from '@angular/forms';
import { TiposAportante, CommonService, Dominio, Localizacion } from 'src/app/core/_services/common/common.service';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';
import { FuenteFinanciacion, FuenteFinanciacionService, CuentaBancaria, RegistroPresupuestal, ControlRecurso, VigenciaAporte } from 'src/app/core/_services/fuenteFinanciacion/fuente-financiacion.service';
import { forkJoin } from 'rxjs';
import { CofinanciacionDocumento } from 'src/app/core/_services/Cofinanciacion/cofinanciacion.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-control-de-recursos',
  templateUrl: './control-de-recursos.component.html',
  styleUrls: ['./control-de-recursos.component.scss']
})
export class ControlDeRecursosComponent implements OnInit {

  addressForm: FormGroup;
  hasUnitNumber = false;
  tipoAportante = TiposAportante;
  nombreAportante: string = '';
  departamento: string = '';
  municipio: string = '';
  vigencia: number;
  nombreFuente: string = '';
  valorFuente: number = 0;
  idFuente: number = 0;
  idControl: number = 0;
  listaNombres: Dominio[] = [];
  listaFuentes: Dominio[] = [];
  listaDepartamentos: Localizacion[] = [];
  listaVigencias: VigenciaAporte[] = [];
  fuente: FuenteFinanciacion;
  fuenteFinaciacionId: number = 0;
  tipoAportanteId: string = '';
  valorAporteEnCuenta: number = 0;
  NombresDeLaCuenta: CuentaBancaria[] = [];
  countResources: any = [];
  rpArray: RegistroPresupuestal[] = [];
  estaEditando = false;
  esVerDetalle = false;
  esVerDetalleRegistro = false;
  listcontrolRecursosArray = [];

  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private fuenteFinanciacionServices: FuenteFinanciacionService,
    private commonService: CommonService,
    private dialog: MatDialog,
    private routes: Router,
  ) {

  }

  ngOnInit(): void {
    this.addressForm = this.fb.group({
      controlRecursoId: [],
      nombreCuenta: [null, Validators.required],
      numeroCuenta: [null, Validators.required],
      rp: [null],
      valorRp: [null],
      vigencia: [null],
      vigenciaValor: [null],
      fechaConsignacion: [null, Validators.required],
      valorConsignacion: [null, Validators.compose([
        Validators.required, Validators.minLength(4), Validators.maxLength(50)])
      ],
    });

    this.addressForm.get('valorConsignacion').valueChanges
      .pipe(debounceTime(400))
      .subscribe(value => {
        this.validar()
      })

    this.activatedRoute.params.subscribe(param => {
      this.idFuente = param['idFuente'];
      this.idControl = param['idControl'];
      this.countResources = [];
      forkJoin([
        this.fuenteFinanciacionServices.getFuenteFinanciacion(this.idFuente),
        this.commonService.listaNombreAportante(),
        this.commonService.listaFuenteTipoFinanciacion(),
        this.commonService.listaDepartamentos()

      ]).subscribe(respuesta => {

        this.fuente = respuesta[0];
        this.listaNombres = respuesta[1];
        this.listaFuentes = respuesta[2];
        this.listaDepartamentos = respuesta[3];
        this.tipoAportanteId = this.fuente.aportante.tipoAportanteId;
        if (this.fuente != null) {
          this.fuente.controlRecurso.forEach(element => {
            if (this.isETOrThirdParty()) {
              this.calculateSumAccountByRp(element);
            } else {
              this.calculateSumAccountByVigency(element);
            }
          });
          if (this.fuente.asociadoASolicitud == true) {
            this.esVerDetalle = true;
          } else {
            this.esVerDetalle = false;
          }
        }
        if (this.tipoAportante.ET.includes(this.tipoAportanteId.toString())) {
          let valorDepartamento = this.listaDepartamentos.find(de => de.localizacionId.toString() ==
            this.fuente.aportante.departamentoId.toString())
          if (valorDepartamento) {
            this.commonService.listaMunicipiosByIdDepartamento(this.fuente.aportante.departamentoId.toString()).subscribe(mun => {
              if (mun) {
                let valorMunicipio = mun.find(m => m.localizacionId == this.fuente.aportante.municipioId.toString())
                this.municipio = valorMunicipio ? valorMunicipio.descripcion.toUpperCase() : "";
              }
            })
            this.departamento = valorDepartamento ? valorDepartamento.descripcion.toUpperCase() : "";
          }


        }
        let valorNombre = this.listaNombres.find(nom => nom.dominioId == this.fuente.aportante.nombreAportanteId);
        console.log(this.fuente);

        let valorFuente = this.listaFuentes.find(fue => fue.codigo == this.fuente.fuenteRecursosCodigo);
        let valorMunicipio: string = '';
        this.nombreFuente = valorFuente.nombre;
        this.valorFuente = this.fuente.valorFuente;
        this.fuenteFinaciacionId = this.fuente.fuenteFinanciacionId;
        this.vigencia = this.fuente.aportante.cofinanciacion.vigenciaCofinanciacionId;
        this.NombresDeLaCuenta = this.fuente.cuentaBancaria;
        this.rpArray = this.fuente.aportante.registroPresupuestal;
        //la lista de vigencias son los documentos registrados en acuerdos de cofinanciacion

        if (this.isETOrThirdParty()) {
          this.fuente.aportante.cofinanciacionDocumento.forEach(element => {
            if (element.vigenciaAporte != null && element.vigenciaAporte != undefined) {
              this.listaVigencias.push({
                tipoVigenciaCodigo: element?.vigenciaAporte.toString(),
                fuenteFinanciacionId: null,
                valorAporte: element?.valorDocumento,
                vigenciaAporteId: element?.cofinanciacionDocumentoId
              });
            }
          });
        } else {
          this.listaVigencias = this.fuente.vigenciaAporte;
        }

        if (this.idControl > 0) {
          this.estaEditando = true;
          this.addressForm.markAllAsTouched();
          this.editMode();
        }

      })
    });
    this.activatedRoute.snapshot.url.forEach((urlSegment: UrlSegment) => {
      if (urlSegment.path === 'verDetalleControlRecursos') {
        this.esVerDetalleRegistro = true;
        return;
      }
    });

  }

  private calculateSumAccountByRp(element: ControlRecurso) {
    let vigency: { value: number, numeroRp: number };
    if (this.countResources.length > 0 && element.registroPresupuestal) {
      vigency = this.countResources.find(x => x.numeroRp === element.registroPresupuestal.numeroRp);
      if (vigency)
        vigency.value = (vigency ? vigency.value : 0) + Number(element.valorConsignacion);
    }
    if (!vigency && element.registroPresupuestal) {
      this.countResources.push({ value: element.valorConsignacion, numeroRp: element.registroPresupuestal.numeroRp, controlRecursoId: element.controlRecursoId });
    }
  }

  private calculateSumAccountByVigency(element: ControlRecurso) {
    let vigency: { value: number, vigenciaAporteId: number };
    if (this.countResources.length > 0) {
      vigency = this.countResources.find(x => x.vigenciaAporteId === element.vigenciaAporteId);
      if (vigency)
        vigency.value = (vigency ? vigency.value : 0) + Number(element.valorConsignacion);
    }
    if (!vigency) {
      this.countResources.push({ value: element.valorConsignacion, vigenciaAporteId: element.vigenciaAporteId, controlRecursoId: element.controlRecursoId });
    }
  }

  editMode() {
    this.fuenteFinanciacionServices.getResourceControlById(this.idControl).subscribe(cr => {
      let cuentaSeleccionada = this.NombresDeLaCuenta.find(c => c.cuentaBancariaId == cr.cuentaBancariaId)
      let rpSeleccionado = this.rpArray.find(rp => rp.registroPresupuestalId == cr.registroPresupuestalId)
      let vigenciaSeleccionada = this.listaVigencias.find(vi => vi.vigenciaAporteId == cr.vigenciaAporteId)
      this.addressForm.get('nombreCuenta').setValue(cuentaSeleccionada);
      this.addressForm.get('rp').setValue(rpSeleccionado);
      if (rpSeleccionado) {
        this.addressForm.get('valorRp').setValue(rpSeleccionado.valorRp);
      }
      this.addressForm.get('controlRecursoId').setValue(cr.controlRecursoId);
      this.addressForm.get('vigencia').setValue(vigenciaSeleccionada);
      if (vigenciaSeleccionada) {
        this.addressForm.get('vigenciaValor').setValue(vigenciaSeleccionada.valorAporte)
      }
      this.addressForm.get('fechaConsignacion').setValue(cr.fechaConsignacion);
      this.addressForm.get('valorConsignacion').setValue(cr.valorConsignacion, { emitEvent: false });
      this.changeNombreCuenta();
    })
  }

  changeNombreCuenta() {
    let cuentaSeleccionada = this.addressForm.get('nombreCuenta').value;
    this.addressForm.get('numeroCuenta').setValue(cuentaSeleccionada.numeroCuentaBanco);
  }

  changeRp() {
    const rpSelected = this.addressForm.get('rp').value;
    this.addressForm.get('valorRp').setValue(rpSelected.valorRp);
  }

  changeVigencia() {
    const vigenciaSelected = this.addressForm.get('vigencia').value;
    this.addressForm.get('vigenciaValor').setValue(vigenciaSelected.valorAporte);
  }

  get vigenciaField() {
    return this.addressForm.get('vigencia');
  }

  get rpField() {
    return this.addressForm.get('rp');
  }
  get vigenciaValorField() {
    return this.addressForm.get('vigenciaValor');
  }

  openDialog(modalTitle: string, modalText: string) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    dialogRef.afterClosed().subscribe(result => {

      this.routes.navigateByUrl('/', { skipLocationChange: true }).then(
        () => this.routes.navigate(
          [
            '/gestionarFuentes/controlRecursos', this.idFuente, 0
          ]
        )
      );

    });
  }

  openDialogError(modalTitle: string, modalText: string) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    dialogRef.afterClosed().subscribe(result => {

    });
  }

  onSubmit() {
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();
    if (this.addressForm.valid) {

      let rp = this.addressForm.get('rp').value;
      let control: any = {
        controlRecursoId: this.addressForm.get('controlRecursoId').value,
        cuentaBancariaId: this.addressForm.get('nombreCuenta').value.cuentaBancariaId,
        fechaConsignacion: this.addressForm.get('fechaConsignacion').value,
        fuenteFinanciacionId: this.fuenteFinaciacionId,
        registroPresupuestalId: rp ? rp.registroPresupuestalId : null,
        valorConsignacion: this.addressForm.get('valorConsignacion').value,
        vigenciaAporteId: this.addressForm.get('vigencia').value?.vigenciaAporteId
      }

      if (control.controlRecursoId > 0)
        this.fuenteFinanciacionServices.updateControlRecurso(control).subscribe(respuesta => {
          this.openDialog('', `<b>${respuesta.message}</b>`);
        })
      else
        this.fuenteFinanciacionServices.registrarControlRecurso(control).subscribe(respuesta => {
          this.openDialog('', `<b>${respuesta.message}</b>`);
        })

    }
  }

  validar() {
    let total = this.valorAporteEnCuenta + this.addressForm.get("valorConsignacion").value;
    let fuenteModificando: ControlRecurso;

    if (!this.isETOrThirdParty()) {
      this.validateVigency(fuenteModificando)
      return;
    } else if (this.isETOrThirdParty()) {
      this.validateRPValue(fuenteModificando);
    }
  }

  isETOrThirdParty = function () {
    return !this.tipoAportante.FFIE.includes(this.tipoAportanteId.toString());
  }

  validateVigency(editingResource: ControlRecurso) {
    const controlRecursoId = this.addressForm.get('controlRecursoId').value
    const valorVigencia = !this.vigenciaValorField.value ? 0 : Number(this.vigenciaValorField.value);
    const vigencia = this.vigenciaField.value;
    const newTotal = this.countResources.find(x => x.vigenciaAporteId === vigencia.vigenciaAporteId)


    if (controlRecursoId && newTotal.controlRecursoId == controlRecursoId) {
      newTotal.value = 0;
    }
    const total = (newTotal ? newTotal.value : 0) + Number(this.addressForm.get("valorConsignacion").value);

    if ((!this.countResources || this.countResources == 0)
      && total > valorVigencia) {
      this.openDialogError('', `El <b> valor de la consignación </b> no debe superar el <b> valor del aporte
      de la vigencia asociado a la fuente. </b>, verifique por favor.`);
      this.addressForm.get("valorConsignacion").setValue(null, { emitEvent: false });
    } else if (total > valorVigencia) {
      this.openDialogError('', `El <b> valor de la consignación </b> supera el monto establecido en la vigencia ${this.vigenciaField.value.tipoVigenciaCodigo} </b>, verifique por favor.`);
      this.addressForm.get("valorConsignacion").setValue(editingResource?.valorConsignacion, { emitEvent: false });
    }
  }

  validateRPValue(editingResource: ControlRecurso) {

    const controlRecursoId = this.addressForm.get('controlRecursoId').value
    const numeroRp = this.addressForm.get("rp").value.numeroRp;
    const valorRp = !this.addressForm.get("valorRp").value ? 0 : Number(this.addressForm.get("valorRp").value);
    const newTotal = this.countResources.find(x => x.numeroRp === numeroRp);

    if (controlRecursoId && newTotal.controlRecursoId == controlRecursoId)
      newTotal.value = 0;


    var SumaValorConsignacionXrp = 0;
    var SumaValorConsignacion = 0;

    this.listcontrolRecursosArray.forEach(element => {
      SumaValorConsignacion += element.valorConsignacion ?? 0;

      if (element.numeroRp === numeroRp && element.fuenteFinanciacionId === this.idFuente)
        SumaValorConsignacionXrp += element.valorConsignacion;
    });

    console.log('SumaValorConsignacionXrp =>', SumaValorConsignacionXrp);
    console.log('SumaValorConsignacionXrp + lo otro=>', SumaValorConsignacionXrp + (Number(this.addressForm.get("valorConsignacion").value)));
    console.log('valor rp =>', valorRp);
    console.log('Suma total rps =>', SumaValorConsignacion);

    if ((Number(this.addressForm.get("valorConsignacion").value) + SumaValorConsignacionXrp) > valorRp) {
      this.openDialogError('', `El <b> valor de la consignación no puede superar el valor del aporte a la fuente de recursos, </b> verifique por favor. `);
      this.addressForm.get("valorConsignacion").setValue(null, { emitEvent: false });
    }

    if (SumaValorConsignacion + (Number(this.addressForm.get("valorConsignacion").value)) > this.valorFuente) {
      this.openDialogError('', `El <b> valor de las consignaciones no puede superar el valor del aporte a la fuente de recursos, </b> verifique por favor. `);
      this.addressForm.get("valorConsignacion").setValue(null, { emitEvent: false });
    }

    if ((Number(this.addressForm.get("valorConsignacion").value)) > this.valorFuente) {
      this.openDialogError('', `El <b> valor de la consignación no puede superar el valor del aporte a la fuente de recursos, </b> verifique por favor. `);
      this.addressForm.get("valorConsignacion").setValue(null, { emitEvent: false });
    }

    const total = (newTotal ? newTotal.value : 0) + Number(this.addressForm.get("valorConsignacion").value);

    if ((!this.countResources || this.countResources.length == 0) && total > valorRp) {
      this.openDialogError('', `El <b> valor de la consignación </b> no debe superar el <b> valor RP asociado a la fuente, </b> verifique por favor.`);
      this.addressForm.get("valorConsignacion").setValue(null, { emitEvent: false });
    } else
      if (total > valorRp) {
        this.openDialogError('', `El <b> valor de la consignación </b> no debe superar el <b> monto establecido en el RP, ${numeroRp} </b> verifique por favor.`);
        this.addressForm.get("valorConsignacion").setValue(editingResource?.valorConsignacion, { emitEvent: false });
      }
  }

  agregar() {
    this.routes.navigate(['/gestionarFuentes/controlRecursos', this.idFuente, 0])
  }

  listcontrolRecursos(event) {
    this.listcontrolRecursosArray = event;
  }

}
