import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Contratacion, ContratacionProyecto } from 'src/app/_interfaces/project-contracting';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { MatRadioChange } from '@angular/material/radio';

@Component({
  selector: 'app-definir-caracteristicas',
  templateUrl: './definir-caracteristicas.component.html',
  styleUrls: ['./definir-caracteristicas.component.scss']
})
export class DefinirCaracteristicasComponent implements OnInit {

  contratacionProyecto: ContratacionProyecto;
  tipoIntervencion: string;
  municipio: string;

  addressForm: FormGroup = this.fb.group({
    completada: null,
    reasignacion: null,
    avanceObra: null,
    porcentajeAvanceObra: [null, Validators.compose([
      Validators.required, Validators.minLength(1), Validators.maxLength(3), Validators.min(1), Validators.max(100)])
    ],
    requiereLicencias: null,
    licenciaVigente: null,
    numeroLicencia: '',
    fechaVigencia: null
  });

  idSolicitud: number;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private commonService: CommonService,
    private projectContractingService: ProjectContractingService,
    public dialog: MatDialog,
    private router: Router,

  ) {
    this.getMunicipio();
  }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      const id = params.id;

      this.projectContractingService.getContratacionProyectoById(id).subscribe(response => {

        this.contratacionProyecto = response;

        setTimeout(() => {
          this.commonService.listaTipoIntervencion()
            .subscribe((intervenciones: any) => {
              console.log(intervenciones);
              for (const intervencion of intervenciones) {
                if (intervencion.codigo === this.contratacionProyecto.proyecto.tipoIntervencionCodigo) {
                  this.tipoIntervencion = intervencion.nombre;
                  break;
                }
              }
            });

          this.addressForm.get('reasignacion').setValue(((this.contratacionProyecto.esReasignacion !== null) ? this.contratacionProyecto.esReasignacion : null));
          this.addressForm.get('avanceObra').setValue(((this.contratacionProyecto.esAvanceobra !== null) ? this.contratacionProyecto.esAvanceobra : null));
          this.addressForm.get('porcentajeAvanceObra').setValue(Number(this.contratacionProyecto.porcentajeAvanceObra));
          this.addressForm.get('requiereLicencias').setValue(((this.contratacionProyecto.requiereLicencia !== null) ? this.contratacionProyecto.requiereLicencia : null));
          this.addressForm.get('licenciaVigente').setValue(((this.contratacionProyecto.licenciaVigente !== null) ? this.contratacionProyecto.licenciaVigente : null));
          this.addressForm.get('numeroLicencia').setValue(this.contratacionProyecto.numeroLicencia);
          this.addressForm.get('fechaVigencia').setValue(this.contratacionProyecto.fechaVigencia);
          this.addressForm.get('completada').setValue(((this.contratacionProyecto.tieneMonitoreoWeb !== null) ? this.contratacionProyecto.tieneMonitoreoWeb : null));

          this.idSolicitud = this.contratacionProyecto.contratacionId;

          console.log(this.contratacionProyecto);

        }, 1000);
      });


    });
  };

  getMunicipio() {
    if (this.router.getCurrentNavigation().extras.replaceUrl || this.router.getCurrentNavigation().extras.skipLocationChange === false) {
      this.router.navigate(['/solicitarContratacion']);
      return;
    }

    this.municipio = this.router.getCurrentNavigation().extras.state.municipio;

  }

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {

    this.contratacionProyecto.esReasignacion = this.addressForm.get('reasignacion').value;
    this.contratacionProyecto.esAvanceobra = this.addressForm.get('avanceObra').value;
    this.contratacionProyecto.porcentajeAvanceObra = this.addressForm.get('porcentajeAvanceObra').value;
    this.contratacionProyecto.requiereLicencia = this.addressForm.get('requiereLicencias').value;
    this.contratacionProyecto.licenciaVigente = this.addressForm.get('licenciaVigente').value;
    this.contratacionProyecto.numeroLicencia = this.addressForm.get('numeroLicencia').value;
    this.contratacionProyecto.fechaVigencia = this.addressForm.get('fechaVigencia').value;
    this.contratacionProyecto.tieneMonitoreoWeb = this.addressForm.get('completada').value;

    this.projectContractingService.createEditContratacionProyecto(this.contratacionProyecto)
      .subscribe(respuesta => {

        this.openDialog('', `<b>${respuesta.message}</b>`);
        console.log(respuesta);
        // tslint:disable-next-line: triple-equals
        if (respuesta.code == '200') {
          this.router.navigate(['/solicitarContratacion/solicitud', this.contratacionProyecto.contratacionId]);
        }

      });

  }

  changeProyectoRequiereLicencia( estado: MatRadioChange ){
    if ( estado.value === false ) {
      this.addressForm.get('licenciaVigente').setValue( null );
      this.addressForm.get('numeroLicencia').setValue( null );
      this.addressForm.get('fechaVigencia').setValue( null );
    }
    //console.log( estado );
  }

}
