import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { CommonService, Dominio, Localizacion } from 'src/app/core/_services/common/common.service';
import { ProjectService, InstitucionEducativa } from 'src/app/core/_services/project/project.service';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-form-solicitar-contratacion',
  templateUrl: './form-solicitar-contratacion.component.html',
  styleUrls: ['./form-solicitar-contratacion.component.scss']
})
export class FormSolicitarContratacionComponent implements OnInit {

  esMultiple: FormControl;

  verBusqueda = false;

  addressForm = this.fb.group({
    tipoInterventor: [null],
    llaveMEN: [null],
    region: [null],
    departamento: [null],
    municipio: [null],
    institucionEducativa: [null],
    sede: [null],
  });

  selectTipoInterventor: Dominio[] = [];

  selectRegion: Localizacion[] = [];

  selectDepartamento: Localizacion[] = [];

  selectMunicipio: Localizacion[] = [];

  selectinstitucionEducativa: InstitucionEducativa[] = [];

  selectSede: InstitucionEducativa[] = [];

  constructor(
                private fb: FormBuilder,
                private commonService: CommonService,
                private projectService: ProjectService,

              ) 
  {
    this.declararEsMultiple();
  }

  ngOnInit(): void {
    
    forkJoin([
      this.commonService.listaTipoIntervencion(),
      this.commonService.listaRegion(),

      //this.projectService..
    ]).subscribe( response => {
      this.selectTipoInterventor = response[0];
      this.selectRegion = response[1]
    })
  }

  private declararEsMultiple() {
    this.esMultiple = new FormControl(['free', Validators.required]);
  }

  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  changeRegion(){
    let region = this.addressForm.get('region').value;
    this.commonService.listaDepartamentosByRegionId( region ? region.localizacionId.toString() : null )
        .subscribe( listaDeptos => {
          this.selectDepartamento = listaDeptos;
        })
  }

  changeDepartamento(){
    let departamento = this.addressForm.get('departamento').value;
    this.commonService.listaMunicipiosByIdDepartamento( departamento ? departamento.localizacionId.toString() : null )
    .subscribe( listaMun => {
      this.selectMunicipio = listaMun;
    })
  }

  changeMunicipio(){
    let municipio = this.addressForm.get('municipio').value;
    this.commonService.listaIntitucionEducativaByMunicipioId( municipio ? municipio.localizacionId.toString() : null )
    .subscribe( listaInst => {
      this.selectinstitucionEducativa = listaInst;
    })
  }

  changeInstitucionE(){
    let instituto: InstitucionEducativa = this.addressForm.get('institucionEducativa').value;
    console.log( instituto );
    this.commonService.listaSedeByInstitucionEducativaId( instituto ? instituto.institucionEducativaSedeId : null )
    .subscribe( listaSedes => {
      this.selectSede = listaSedes;
    })
  }

  onSubmit() {
    console.log(this.addressForm.value);
    let pTipoIntervencion: Dominio = this.addressForm.get('tipoInterventor').value;
    let pLlaveMen: string = this.addressForm.get('llaveMEN').value;
    let pMunicipio: Localizacion = this.addressForm.get('municipio').value;
    let pIdInstitucionEducativa: InstitucionEducativa = this.addressForm.get('institucionEducativa').value;
    let pIdSede: InstitucionEducativa = this.addressForm.get('sede').value;

    this.projectService.listaProyectoConFiltros( 
                                                  pTipoIntervencion ? pTipoIntervencion.codigo : null, 
                                                  pLlaveMen, 
                                                  pMunicipio ? pMunicipio.localizacionId : null, 
                                                  pIdInstitucionEducativa ? pIdInstitucionEducativa.institucionEducativaSedeId : null, 
                                                  pIdSede ? pIdSede.institucionEducativaSedeId : null 

                                               ).subscribe( proyectos => {
                                                  console.log( proyectos );
                                               })
  }
}
