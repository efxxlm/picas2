import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormControl, Validators, FormGroup } from '@angular/forms';
import { CommonService, Dominio, Localizacion } from 'src/app/core/_services/common/common.service';
import { ProjectService, InstitucionEducativa, ProyectoGrilla } from 'src/app/core/_services/project/project.service';
import { forkJoin } from 'rxjs';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-form-solicitar-contratacion',
  templateUrl: './form-solicitar-contratacion.component.html',
  styleUrls: ['./form-solicitar-contratacion.component.scss']
})
export class FormSolicitarContratacionComponent implements OnInit {

  esMultiple: FormControl;

  verBusqueda = false;

  addressForm: FormGroup = this.fb.group({
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

  listaResultado: ProyectoGrilla[] = [];
  estaEditando = false;

  constructor(
    private fb: FormBuilder,
    private commonService: CommonService,
    private projectService: ProjectService,
    private dialog: MatDialog
  ) {
    this.declararEsMultiple();
  }

  ngOnInit(): void {

    forkJoin([
      this.commonService.listaTipoIntervencion(),
      this.commonService.listaRegion(),
      // this.projectService..
    ]).subscribe(response => {
      this.selectTipoInterventor = response[0];
      this.selectRegion = response[1];
    });
  }

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  reiniciarFiltro() {
    this.addressForm.setValue({
      tipoInterventor: null,
      llaveMEN: null,
      region: null,
      departamento: null,
      municipio: null,
      institucionEducativa: null,
      sede: null
    });
  }

  private declararEsMultiple() {
    this.esMultiple = new FormControl('free', Validators.required);
  }

  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  changeRegion() {
    const region = this.addressForm.get('region').value;
    this.commonService.listaDepartamentosByRegionId(region ? region.localizacionId.toString() : null)
      .subscribe(listaDeptos => {
        this.selectDepartamento = listaDeptos;
      });
  }

  changeDepartamento() {
    const departamento = this.addressForm.get('departamento').value;
    this.commonService.listaMunicipiosByIdDepartamento(departamento ? departamento.localizacionId.toString() : null)
      .subscribe(listaMun => {
        this.selectMunicipio = listaMun;
      });
  }

  changeMunicipio() {
    const municipio = this.addressForm.get('municipio').value;
    this.commonService.listaIntitucionEducativaByMunicipioId(municipio ? municipio.localizacionId.toString() : null)
      .subscribe(listaInst => {
        this.selectinstitucionEducativa = listaInst;
      });
  }

  changeInstitucionE() {
    const instituto: InstitucionEducativa = this.addressForm.get('institucionEducativa').value;
    console.log(instituto);
    this.commonService.listaSedeByInstitucionEducativaId(instituto ? instituto.institucionEducativaSedeId : null)
      .subscribe(listaSedes => {
        this.selectSede = listaSedes;
      });
  }

  onSubmit() {
    this.estaEditando = true;
    const pTipoIntervencion: Dominio = this.addressForm.get('tipoInterventor').value;
    const pLlaveMen: string = this.addressForm.get('llaveMEN').value;
    const pRegion: Localizacion = this.addressForm.get('region').value;
    const pDepartamento: Localizacion = this.addressForm.get('departamento').value;
    const pMunicipio: Localizacion = this.addressForm.get('municipio').value;
    const pIdInstitucionEducativa: InstitucionEducativa = this.addressForm.get('institucionEducativa').value;
    const pIdSede: InstitucionEducativa = this.addressForm.get('sede').value;

    this.projectService.listaProyectoConFiltros(
      pTipoIntervencion ? pTipoIntervencion.codigo : '',
      pLlaveMen ? pLlaveMen : '',
      pRegion ? pRegion.localizacionId === '7' ? '' : pRegion.localizacionId : '',
      pDepartamento ? pDepartamento.localizacionId : '',
      pMunicipio ? pMunicipio.localizacionId : '',
      pIdInstitucionEducativa ? pIdInstitucionEducativa.institucionEducativaSedeId : 0,
      pIdSede ? pIdSede.institucionEducativaSedeId : 0

    ).subscribe(proyectos => {
      this.listaResultado = proyectos;
      if (proyectos.length === 0) {
        this.openDialog('', '<b>No se encontraron registros asociados a los criterios de búsqueda.</b>');
        return;
      }
      setTimeout(() => {
        const btnListaResultado = document.getElementById('btnListaResultado');
        btnListaResultado.click();
      }, 1000);
    });
  }
}
