import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray, ControlValueAccessor, FormGroup, FormControl } from '@angular/forms';
import { CommonService, Dominio, Localizacion, TiposAportante } from 'src/app/core/_services/common/common.service';
import { CofinanciacionService, CofinanciacionAportante, CofinanciacionDocumento } from 'src/app/core/_services/Cofinanciacion/cofinanciacion.service';
import { forkJoin, from } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { FuenteFinanciacionService, FuenteFinanciacion, CuentaBancaria, RegistroPresupuestal, VigenciaAporte } from 'src/app/core/_services/fuenteFinanciacion/fuente-financiacion.service';
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
  
  mostrarNombreaportante: boolean;
  listaDocumentosApropiacion: CofinanciacionDocumento[];
  tipoDocumentoap: Dominio[]=[];
  nombresAportantes2: CofinanciacionAportante[];
  solonombres: any[]=[];
  edicion: boolean;
  fuentesDeRecursosListaArr: any[]=[];

  constructor(private fb: FormBuilder,
              private commonService: CommonService,
              private cofinanciacionService: CofinanciacionService,
              private activatedRoute: ActivatedRoute,
              public dialog: MatDialog,
              private fuenteFinanciacionService: FuenteFinanciacionService,
              private router: Router,
  ) {
    this.maxDate = new Date();
  }


  openDialog(modalTitle: string, modalText: string,redirect?:boolean) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
    if(redirect)
    {
      dialogRef.afterClosed().subscribe(result => {
        
          this.router.navigate(["/gestionarFuentes"], {});
        
      });
    }
  }
  openDialogSiNo(modalTitle: string, modalText: string,borrarForm: any, i: number,j:number,event:number) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText,siNoBoton:true }
    });   
    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if(result === true)
      {
        if(event==1)
        {
          this.removeItemVigencia(borrarForm,i,j);
        }
        else{
          if(event==3)
          {
            this.cuentasBancaria(i).removeAt(j);  
            this.openDialog("","<b>La información a sido eliminada correctamente.</b>",false);
          }
          else{
            this.borrarVigencia(borrarForm,i);
          }
          
        }
      }           
    });
  }

  EditMode() {

    this.fuenteRecursosArray.clear();
    const aportante = this.nombresAportantes.find(nom => nom.cofinanciacionAportanteId == this.idAportante);
    console.log("este es el apo");
    console.log(aportante);
    this.addressForm.get('tieneRP').setValue(aportante.cuentaConRp==true?"1":"0");
    if(this.tipoAportante.FFIE.includes(this.tipoAportanteId.toString()))
    {
      this.addressForm.get('nombreAportanteFFIE').setValue(aportante);  
        this.changeNombreAportanteFFIE();            
    }
    else if(this.tipoAportante.ET.includes(this.tipoAportanteId.toString())){
      
      if(aportante.municipioId!=null){
        this.commonService.listaMunicipiosByIdDepartamento(aportante.departamentoId.toString()).subscribe(mun => {
          const valorMunicipio = mun.find(a => a.localizacionId === aportante.municipioId.toString());
          const valorDepartamento = this.departamentos.find(a => a.localizacionId == aportante.departamentoId.toString());
  
          this.municipios = mun;
          this.addressForm.get('municipio').setValue(valorMunicipio);
          this.addressForm.get('departamento').setValue(valorDepartamento);
          this.changeMunicipio();
        });
      }
      else{
        const valorDepartamento = this.departamentos.find(a => a.localizacionId == aportante.departamentoId.toString());
        this.addressForm.get('departamento').setValue(valorDepartamento);
        this.changeDepartamento();
      }
      
      this.addressForm.get('vigenciaAcuerdo').setValue(aportante);
      
      this.changeVigencia();
    }
    else{      
      this.addressForm.get('nombreAportante').setValue(aportante.nombreAportanteString);
      this.changeNombreAportanteTercero();
      this.addressForm.get('vigenciaAcuerdo').setValue(aportante);
      this.changeVigencia();
    }

    
    const listaRegistrosP = this.addressForm.get('registrosPresupuestales') as FormArray;

    
    this.fuenteFinanciacionService.listaFuenteFinanciacionByAportante(this.idAportante).subscribe(lista => {
      
      console.log("esta es la lista de fuentes");
      console.log(lista);
      let i=0;
      lista.forEach(ff => {          
        this.fuentesDeRecursosListaArr.push(this.fuentesDeRecursosLista);
        if(this.tipoAportante.FFIE.includes(this.tipoAportanteId.toString()))
        {
          
          const tipo = this.tipoDocumentoap.filter(x=>x.dominioId==ff.cofinanciacionDocumento.tipoDocumentoId);      
          this.addressForm.get('tipoDocumento').setValue(tipo[0].dominioId);        
          const numerodoc = this.listaDocumentosApropiacion.filter(x=>x.cofinanciacionDocumentoId==ff.cofinanciacionDocumentoId);        
          this.addressForm.get('numerodocumento').setValue(numerodoc[0]);
        }
        const grupo: FormGroup = this.crearFuenteEdit(ff.valorFuente);
        const fuenteRecursosSeleccionada = this.fuentesDeRecursosListaArr[i].find(f => f.codigo === ff.fuenteRecursosCodigo);
        i++;
        const listaVigencias = grupo.get('vigencias') as FormArray;
        const listaCuentas = grupo.get('cuentasBancaria') as FormArray;

        grupo.get('cuantasVigencias').setValue(ff.cantVigencias);
        grupo.get('fuenteRecursos').setValue(fuenteRecursosSeleccionada);
        grupo.get('fuenteFinanciacionId').setValue(ff.fuenteFinanciacionId);

        // Vigencias
        ff.vigenciaAporte.forEach(v => {
          const grupoVigencia = this.createVigencia();
          const vigenciaSeleccionada = this.VigenciasAporte.find(vtemp => vtemp == v.tipoVigenciaCodigo);

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
        });
        

        this.fuenteRecursosArray.push(grupo);
      });
      // Registro Presupuestal
      lista[0].aportante.registroPresupuestal.forEach(rp => {
        const grupoRegistroP = this.createRP();

        grupoRegistroP.get('registroPresupuestalId').setValue(rp.registroPresupuestalId);
        grupoRegistroP.get('numeroRP').setValue(rp.numeroRp);
        grupoRegistroP.get('fecha').setValue(rp.fechaRp);
        console.log("busco "+rp.cofinanciacionDocumentoId);
        console.log(this.listaDocumentos);
        let documentose=this.listaDocumentos.filter(x=>x.cofinanciacionDocumentoId==rp.cofinanciacionDocumentoId);
        console.log(documentose);
        grupoRegistroP.get('numerodocumentoRP').setValue(documentose[0]);

        this.addressForm.get('cuantosRP').setValue(lista[0].aportante.registroPresupuestal.length);

        listaRegistrosP.push(grupoRegistroP);

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
      fuenteFinanciacionId: [null, Validators.required],
      cuentasBancaria: this.fb.array([
      ]),
      tieneRP: [null, Validators.required],
      cuantosRP: [null, Validators.compose([
        Validators.minLength(5), Validators.maxLength(50)])]

    });
  }

  changeMunicipio(){    
    const idMunicipio = this.addressForm.get('municipio').value.localizacionId;
    console.log(idMunicipio);
    this.nombresAportantes2=this.nombresAportantes.filter(z=>z.municipioId==idMunicipio);
    if(this.nombresAportantes2.length==0)
    {
      this.openDialog("","<b>No tiene acuerdos disponibles.</b>");
    }
  }

  changeVigencia()
  {
    const vigencia = this.addressForm.get('vigenciaAcuerdo').value;
    console.log(vigencia);
    this.idAportante=vigencia.cofinanciacionAportanteId;
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
        this.cofinanciacionService.listaAportantesByTipoAportante(this.tipoAportanteId),
        //this.fuenteFinanciacionService.listaFuenteFinanciacion(),
        
      ]).subscribe(res => {

        this.nombresAportantes = res[4].filter(x=>x.cofinanciacion.registroCompleto==true);//solo muestro los completos
        if(this.idAportante>0)//funciona porque recien empezo
        {          
          this.edicion=true;        
        }
        else
        {          
          this.edicion=false;
          this.nombresAportantes =this.nombresAportantes.filter(x=>x.fuenteFinanciacion.length==0) ;
          this.fuentesDeRecursosListaArr.push(res[1]);
        }
        const nombresAportantesTemp: Dominio[] = res[0];

        this.nombresAportantes.forEach(nom => {
          if(!this.solonombres.includes(nom.nombreAportanteString))
          {
            this.solonombres.push(nom.nombreAportanteString);
          }          
        });

        this.fuentesDeRecursosLista = res[1];
        
        this.bancos = res[2];
        this.departamentos = res[3];
        //this.listaFuentes = res[5];

        if (this.idAportante > 0) {
          this.edicion=true;       
          this.EditMode();
        }

      });
    });



    this.addressForm = this.fb.group({
      nombreAportante: [null, Validators.required],
      nombreAportanteFFIE:[null, Validators.required],
      documentoApropiacion: [null, Validators.required],
      tipoDocumento:[null, Validators.required],
      numerodocumento: [null, Validators.compose([
        Validators.minLength(10), Validators.maxLength(10)])
      ],
      vigenciaAcuerdo: [null, Validators.required],
      departamento: [null, Validators.required],
      municipio: [null, Validators.required],
      tieneRP: [null, Validators.required],
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
      registroPresupuestalId: [null, Validators.required],
      numeroRP: [null, Validators.required],
      fecha: [null, Validators.required],
      numerodocumentoRP:[null, Validators.required]
    });
  }

  CambioNumeroRP() {
    const FormNumRP = this.addressForm.get('cuantosRP').value;
    if (FormNumRP > this.registrosPresupuestales.length && FormNumRP < 100) {
      while (this.registrosPresupuestales.length < FormNumRP) {
        this.registrosPresupuestales.push(this.createRP());
      }
    } else if (FormNumRP <= this.registrosPresupuestales.length && FormNumRP >= 0) {
      let i=this.registrosPresupuestales.length;
      while (this.registrosPresupuestales.length > FormNumRP) {
        console.log(this.registrosPresupuestales.length);
        console.log(FormNumRP)
        this.registrosPresupuestales.removeAt(i);
        i--;
      }
    }
  }

  changeDepartamento() {
    console.log("change departamento");
    console.log(this.nombresAportantes);
    const idDepartamento = this.addressForm.get('departamento').value.localizacionId;
    this.nombresAportantes2=this.nombresAportantes.filter(z=>z.departamentoId==idDepartamento && z.municipioId==null);
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
          numeroAcuerdo:doc.numeroAcuerdo,
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

  changeNombreAportanteTercero()
  {
    if (this.addressForm.get('nombreAportante').value) {
      this.nombresAportantes2=this.nombresAportantes.filter(z=>z.nombreAportanteString==this.addressForm.get('nombreAportante').value);
      if(this.nombresAportantes2.length==0)
      {
        this.openDialog("","<b>No tiene acuerdos disponibles.</b>");
      }
    }
  }

  changeNombreAportante() {
    if (this.addressForm.get('nombreAportante').value) {

      this.idAportante = this.addressForm.get('nombreAportante').value.cofinanciacionAportanteId?
        this.addressForm.get('nombreAportante').value.cofinanciacionAportanteId:
        this.addressForm.get('nombreAportanteFFIE').value.cofinanciacionAportanteId;


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
        //const vigenciaSeleccionada = this.addressForm.get('nombreAportante').value.cofinanciacion.vigenciaCofinanciacionId;
        //const idMunicipio = this.addressForm.get('nombreAportante').value.municipioId;
        //const vigenciaRegistro = this.VigenciasAporte.find(vtemp => vtemp === vigenciaSeleccionada);
        //this.addressForm.get('vigenciaAcuerdo').setValue(vigenciaRegistro);
        this.cargarDocumentos();
      }
    }
  }


  changeNombreAportanteFFIE()
  {
    if (this.addressForm.get('nombreAportanteFFIE').value) {

      this.idAportante = 
        this.addressForm.get('nombreAportanteFFIE').value.cofinanciacionAportanteId;

      // FFIE
      if (this.tipoAportante.FFIE.includes(this.tipoAportanteId.toString())) {
        this.cofinanciacionService.getDocumentoApropiacionByAportante(this.idAportante).subscribe(listDoc => {
          if (listDoc.length > 0) {
            //this.addressForm.get('numerodocumento').setValue(listDoc[0].numeroActa);
            //this.addressForm.get('documentoApropiacion').setValue(listDoc[0].tipoDocumento.nombre);
            this.listaDocumentosApropiacion=listDoc;
            listDoc.forEach(element => {
              let m = this.tipoDocumentoap.some(function(item) {
                return item.dominioId === element.tipoDocumentoId
              });
              if(m)
              {
                console.log("ya lo tiene");
              }
              else
              {
                this.tipoDocumentoap.push({dominioId:element.tipoDocumentoId,nombre:element.tipoDocumento.nombre});
              }              
            });
          }
          else {
            this.openDialog('Validacion', 'No tiene documentos de apropiacion');
          }
        });
      }

    }
  }


  get fuenteRecursosArray() {
    return this.addressForm.get('fuenteRecursosArray') as FormArray;
  }
  vigencias1(i:number) {
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
        if(FormNumvigencias.cuantasVigencias==1)
        {
          this.vigencias1(j).push(this.fb.group({
            vigenciaAporteId: [null, Validators.required],
            vigenciaAportante: [null, Validators.required],
            valorVigencia: [this.addressForm.get("fuenteRecursosArray")['controls'][j].value.valorFuenteRecursos, Validators.compose([
              Validators.minLength(10), Validators.maxLength(10)])
            ]
          }));
        }
        else
        {
          while (this.vigencias1(j).length < FormNumvigencias.cuantasVigencias) {
            this.vigencias1(j).push(this.createVigencia());
          }  
        }
        
      } else if (FormNumvigencias.cuantasVigencias <= this.vigencias1(j).length && FormNumvigencias.cuantasVigencias >= 0) {
        //valido si tiene data
        let bitestavacio=true;
        FormNumvigencias.vigencias.forEach(element => {
          if(element.valorVigencia!=null || element.vigenciaAportante!=null ||element.vigenciaAporteId!=null)
          {
            bitestavacio=false
          }
        });
        if(bitestavacio)
        {          
          let cuantas=FormNumvigencias.cuantasVigencias;
          var resta=this.vigencias1(j).length-FormNumvigencias.cuantasVigencias
          for(let secuenciaa=this.vigencias1(j).length;secuenciaa>=cuantas;secuenciaa--) {
            //this.removeItemVigencia(this.vigencias1(j), this.vigencias1(j).length - 1,j,false);
            this.vigencias1(j).removeAt(secuenciaa);
          }
          //this.addressForm.get("fuenteRecursosArray")['controls'][j].get("cuantasVigencias").setValue(this.vigencias1(j).length);
        }
        else{
          this.openDialog("","<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>");
          this.addressForm.get("fuenteRecursosArray")['controls'][j].get("cuantasVigencias").setValue(this.vigencias1(j).length);
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
    console.log("es edición"+this.edicion);         
    if(this.addressForm.value.fuenteRecursosArray.length==2)
    {
      this.openDialog("","<b>Ya cuenta con los dos tipos de fuente de financiación disponibles.</b>");
      return;
    }
      this.addressForm.value.fuenteRecursosArray.forEach(element => {      
        this.fuentesDeRecursosListaArr.push(this.fuentesDeRecursosLista.filter(x=>x!=element.fuenteRecursos));
      });              
    console.log(this.fuentesDeRecursosListaArr);
    this.fuenteRecursosArray.push(this.crearFuente());
  }

  createCuentaBancaria(): FormGroup {
    return this.fb.group({
      cuentaBancariaId: [null, Validators.required],
      numeroCuenta: [null, Validators.compose([
        Validators.required, Validators.minLength(1), Validators.maxLength(50)])
      ],
      nombreCuenta: [null, Validators.compose([
        Validators.required, Validators.minLength(1), Validators.maxLength(100)])
      ],
      codigoSIFI: [null, Validators.compose([
        Validators.required, Validators.minLength(1), Validators.maxLength(6)])
      ],
      tipoCuenta: [null, Validators.required],
      banco: [null, Validators.required],
      extra: [null, Validators.required]
    });
  }

  crearFuente(): FormGroup {
    
    return this.fb.group({
      fuenteRecursos: [null, Validators.required],
      valorFuenteRecursos: [null, Validators.compose([
        Validators.required, Validators.minLength(2), Validators.maxLength(2)])
      ],
      cuantasVigencias: [null, Validators.required],
      vigencias: this.fb.array([]),
      fuenteFinanciacionId: [null, Validators.required],
      departamento: [null, Validators.required],
      municipio: [null, Validators.required],
      tieneRP: [null, Validators.required],
      cuantosRP: [null, Validators.compose([
        Validators.minLength(5), Validators.maxLength(50)])]
      , cuentasBancaria: this.fb.array([
        this.fb.group({
          cuentaBancariaId: [null, Validators.required],
          numeroCuenta: [null, Validators.compose([
            Validators.required, Validators.minLength(1), Validators.maxLength(50)])
          ],
          nombreCuenta: [null, Validators.compose([
            Validators.required, Validators.minLength(1), Validators.maxLength(100)])
          ],
          codigoSIFI: [null, Validators.compose([
            Validators.required, Validators.minLength(1), Validators.maxLength(6)])
          ],
          tipoCuenta: [null, Validators.required],
          banco: [null, Validators.required],
          extra: [null, Validators.required]
        })
      ]),
    });
  }

  createVigencia(): FormGroup {
    return this.fb.group({
      vigenciaAporteId: [null, Validators.required],
      vigenciaAportante: [null, Validators.required],
      valorVigencia: [null, Validators.compose([
        Validators.minLength(10), Validators.maxLength(10)])
      ]
    });
  }

  borrarArray(borrarForm: any, i: number,tipo:number,posicion:number) {
    
    this.openDialogSiNo("","<b>¿Está seguro de eliminar este registro?</b>",borrarForm,i,posicion,tipo);
      
  }
  borrarVigencia(borrarForm: any, i: number)
  {
    console.log(borrarForm);console.log(i);
    borrarForm.removeAt(i);  
    this.openDialog("","<b>La información a sido eliminada correctamente.</b>",false);
  }
  borrarArrayVigencias(borrarForm: any, i: number,j:number) {    
    this.openDialogSiNo("","<b>¿Está seguro de eliminar este registro?</b>",borrarForm,i,j,1);
  }
  removeItemVigencia(borrarForm: any, i: number,j:number,mensaje=true)
  {
    borrarForm.removeAt(i);
    this.addressForm.get("fuenteRecursosArray")['controls'][j].get("cuantasVigencias").setValue(this.vigencias1(i).length);    
    if(mensaje)
    {
      this.openDialog("","<b>La información a sido eliminada correctamente.</b>",false);
    }
    
  }
  // evalua tecla a tecla
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  filterDocumento(variable)
  {
    console.log(variable);
    this.listaDocumentosApropiacion=this.listaDocumentosApropiacion.filter(x=>x.tipoDocumentoId==variable);
  }

  onSubmit() {
    if (this.addressForm.valid) {
      
      let bitValorok=true;
      const lista: FuenteFinanciacion[] = [];
      const listaRP: RegistroPresupuestal[] = [];
  
      let usuario = '';
      if (localStorage.getItem('actualUser')) {
        usuario = localStorage.getItem('actualUser');
        usuario = JSON.parse(usuario).email;
      }
      console.log(this.addressForm.get('numerodocumento'));
      let valortotla=0;
      let valorBase=this.valorTotal;
      this.fuenteRecursosArray.controls.forEach(controlFR => {      
        const vigencias = controlFR.get('vigencias') as FormArray;
        if (vigencias.controls.length==0) {          
        valortotla+=controlFR.get('valorFuenteRecursos').value;
        }
        const fuente: FuenteFinanciacion = {
          fuenteFinanciacionId: controlFR.get('fuenteFinanciacionId').value,
          aportanteId: this.idAportante,
          fuenteRecursosCodigo: controlFR.get('fuenteRecursos').value.codigo,
          valorFuente: controlFR.get('valorFuenteRecursos').value,
          cantVigencias: controlFR.get('cuantasVigencias').value,
          cuentaBancaria: [],
          vigenciaAporte: [],
          cofinanciacionDocumentoId:this.addressForm.get('numerodocumento').value==null?0:this.addressForm.get('numerodocumento').value.cofinanciacionDocumentoId,
          aportante:{
            cofinanciacionAportanteId:this.idAportante,
            cuentaConRp:this.addressForm.get('tieneRP').value=="1"?true:false,
            cofinanciacionId: null,
            tipoAportanteId: null,
            municipioId: null,
            departamentoId:null,
            cofinanciacionDocumento:null,
          }
        };

        
        console.log("vigencias "+vigencias.controls.length);
        if (vigencias.controls.length>0) {

          let totalaportes=0;
          vigencias.controls.forEach(controlVi => {
            const vigenciaAporte: VigenciaAporte = {
              vigenciaAporteId: controlVi.get('vigenciaAporteId').value,
              fuenteFinanciacionId: controlFR.get('fuenteFinanciacionId').value,
              tipoVigenciaCodigo: controlVi.get('vigenciaAportante').value,
              valorAporte: controlVi.get('valorVigencia').value,

            };
            totalaportes+=controlVi.get('valorVigencia').value;
            valortotla+=controlVi.get('valorVigencia').value;
            fuente.vigenciaAporte.push(vigenciaAporte);
            
            
          });     
            //si tengo vigencias mi valor base es la fuente
            valorBase+=controlFR.get('valorFuenteRecursos').value;

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
              cofinanciacionDocumentoId:controlRP.get('numerodocumentoRP').value.cofinanciacionDocumentoId,
            };
            listaRP.push(registroPresupuestal);
          });
        }

        lista.push(fuente);
      });
      if(valorBase!=valortotla)
          {            
            console.log(valorBase+" vs "+valortotla);
            this.openDialog("","<b>Los valores de aporte de las vigencias son diferentes al valor de aporte de la fuente.</b>");
            bitValorok=false;
            return false;
          }
      if(bitValorok)
      {
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
              this.openDialog('', res.message,true);
            }
            console.log(respuesta);
          });      
        this.data = lista;    
      }
      
    }
  }
  
  validateonevige(j){
    console.log(this.addressForm.get("valorFuenteRecursos"));
    console.log(this.vigencias1(j).controls.length);
    const FormNumvigencias = this.addressForm.value.fuenteRecursosArray[j];
    if(FormNumvigencias.cuantasVigencias==1)
    {
      console.log("lo seteo");
      this.vigencias1(j).controls[0].get("valorVigencia").setValue(FormNumvigencias.valorFuenteRecursos);
    }
  }
  changefuenteRecursos(j)
  {
    let fuentes=[];
    this.addressForm.value.fuenteRecursosArray.forEach(element => {
      console.log(element.fuenteRecursos);
      if(fuentes.includes(element.fuenteRecursos))
      {
        this.openDialog("","<b>No puedes tener dos tipos de fuentes iguales por aportante</b>");
        this.addressForm.value.fuenteRecursosArray[j].fuenteRecursos=null;
      }
      else{
        fuentes.push(element.fuenteRecursos);
      }      
    });
    console.log();
  }
}
