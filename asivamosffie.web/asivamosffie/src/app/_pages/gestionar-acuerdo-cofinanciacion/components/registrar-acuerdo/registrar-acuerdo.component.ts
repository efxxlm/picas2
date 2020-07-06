import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray, ControlValueAccessor, FormGroup, FormControl } from '@angular/forms';
import { CofinanciacionService, CofinanciacionAportante, Cofinanciacion, CofinanciacionDocumento } from 'src/app/core/_services/Cofinanciacion/cofinanciacion.service';
import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';
import { ClassGetter } from '@angular/compiler/src/output/output_ast';
import { Console } from 'console';

@Component({
  selector: 'app-registrar-acuerdo',
  templateUrl: './registrar-acuerdo.component.html',
  styleUrls: ['./registrar-acuerdo.component.scss']
})
export class RegistrarAcuerdoComponent implements OnInit {

  mostrarDocumentosDeApropiacion = true;
  maxDate: Date;
  vigenciaEstados: number[];
  vigenciasAportante: number[];
  tiposDocumento: Dominio[]; 
  selectTiposAportante: Dominio[];
  nombresAportante: Dominio[];
  valorTotalAcuerdo = 85000000;

  constructor(private fb: FormBuilder,
              private cofinanciacionService: CofinanciacionService,
              private commonService: CommonService) {
    this.maxDate = new Date();

    this.datosAportantes.get('numAportes').valueChanges
    .subscribe( () => {
      this.CambioNumeroAportantes();
    });
  }

  ngOnInit(): void {
    this.vigenciasAportante = this.cofinanciacionService.vigenciasAcuerdoCofinanciacion();
    this.vigenciaEstados = this.cofinanciacionService.vigenciasAcuerdoCofinanciacion();
    
    this.commonService.listaTipoDocFinanciacion().subscribe( doc => { this.tiposDocumento = doc; });
    this.commonService.listaTipoAportante().subscribe( apo => {this.selectTiposAportante = apo; });
    this.commonService.listaNombreAportante().subscribe( nom => {this.nombresAportante = nom; });

  }

  get aportantes() {
    return this.datosAportantes.get('aportantes') as FormArray;
  }

  get DocumentoAportantes() {
    return this.documentoApropiacion.get('aportantes') as FormArray;
  }

  datosAportantes = this.fb.group({
    vigenciaEstado: ['', Validators.required],
    numAportes: ['', [Validators.required, Validators.maxLength(2), Validators.min(1), Validators.max(99)]],
    aportantes: this.fb.array([])
  });

  // tabla de los documentos de aportantes
  documentoApropiacion = this.fb.group({
    aportantes: this.fb.array([])
  });

  CambioNumeroAportantes(){
    const FormNumAportantes = this.datosAportantes.value;
    if (FormNumAportantes.numAportes > this.aportantes.length && FormNumAportantes.numAportes < 100) {
      while (this.aportantes.length < FormNumAportantes.numAportes) {
        this.aportantes.push( this.createAportante() );
        this.DocumentoAportantes.push( this.createDocumentoAportante() );
      }
    } else if (FormNumAportantes.numAportes <= this.aportantes.length && FormNumAportantes.numAportes >= 0) {
      while (this.aportantes.length > FormNumAportantes.numAportes) {
        this.borrarAportante(this.aportantes, this.aportantes.length - 1);
        this.borrarAportante(this.DocumentoAportantes, this.aportantes.length - 1);
      }
    }
  }

  createAportante(): FormGroup {
    return this.fb.group({
      tipo: ['', Validators.required],
      nombre: ['', Validators.required],
      cauntosDocumentos: ['', [Validators.required, Validators.maxLength(2), Validators.min(1), Validators.max(99)]],
    });
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
  }

  listaDocumentos( controles: FormArray )
  {
    let lista: CofinanciacionDocumento[] = [];

    controles.controls.forEach(doc => {
      let cofDoc: CofinanciacionDocumento = {};
      //cofDoc.FechaActa = new Date;
      cofDoc.FechaAcuerdo = new Date;
      cofDoc.NumeroActa = '111';
      //cofDoc.NumeroAcuerdo
      cofDoc.TipoDocumentoId = 1;
      cofDoc.ValorDocumento = '110011';
      cofDoc.ValorTotalAportante = '55444';
      cofDoc.VigenciaAporteId = 2017;

      lista.push(cofDoc);
    });

    return lista;
  }

  listaAportantes()
  {
    let lista: CofinanciacionAportante[] = [];
    this.aportantes.controls.forEach( control => 
      {
        let cofiApo: CofinanciacionAportante = {};
        cofiApo.tipoAportanteId = control.get('tipo').value;
        cofiApo.nombreAportanteId = control.get('nombre').value;

        //cofiApo.cofinanciacionDocumento = this.listaDocumentos( control )
                
        lista.push(cofiApo);
      });

      return lista;
  }

  onSubmitDatosAportantes() {
    if (this.datosAportantes.valid) {

      let cofinanciacion: Cofinanciacion = 
      {
        vigenciaCofinanciacionId: this.datosAportantes.get('vigenciaEstado').value,
      }

      cofinanciacion.cofinanciacionAportante = this.listaAportantes();

      this.cofinanciacionService.CrearOModificarAcuerdoCofinanciacion(cofinanciacion).subscribe( console.log );

      this.mostrarDocumentosDeApropiacion = true;
    }
  }

  onSubmitDocumentosAportantes() {
    console.log(this.documentoApropiacion.value);
  }
}
