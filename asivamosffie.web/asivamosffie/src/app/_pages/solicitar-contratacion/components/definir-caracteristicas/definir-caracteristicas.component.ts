import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Contratacion, ContratacionProyecto } from 'src/app/_interfaces/project-contracting';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-definir-caracteristicas',
  templateUrl: './definir-caracteristicas.component.html',
  styleUrls: ['./definir-caracteristicas.component.scss']
})
export class DefinirCaracteristicasComponent implements OnInit {

  contratacionProyecto: ContratacionProyecto;

  addressForm = this.fb.group({
    completada: null,
    reasignacion: null,
    avanceObra: null,
    porcentajeAvanceObra: [null, Validators.compose([
      Validators.required, Validators.minLength(1), Validators.maxLength(3), Validators.min(1), Validators.max(100)])
    ],
    requiereLicencias: null,
    licenciaVigente: null,
    numeroLicencia: null,
    fechaVigencia: null
  });

  idSolicitud: number;

  constructor(
              private fb: FormBuilder,
              private route: ActivatedRoute,
              private projectContractingService: ProjectContractingService,
              public dialog: MatDialog,    
              private router: Router,
              
             ) 
  {}

    ngOnInit(): void {
      this.route.params.subscribe((params: Params) => {
        const id = params.id;

        this.projectContractingService.getContratacionProyectoById( id ).subscribe( response => {

          this.contratacionProyecto = response;

          setTimeout(() => {

            this.addressForm.get('reasignacion').setValue( this.contratacionProyecto.esReasignacion ? this.contratacionProyecto.esReasignacion.toString() : false );    
            this.addressForm.get('avanceObra').setValue( this.contratacionProyecto.esAvanceObra ? this.contratacionProyecto.esAvanceObra.toString() : false );    
            this.addressForm.get('porcentajeAvanceObra').setValue( this.contratacionProyecto.porcentajeAvanceObra );    
            this.addressForm.get('requiereLicencias').setValue( this.contratacionProyecto.requiereLicencia ? this.contratacionProyecto.requiereLicencia.toString() : false );    
            this.addressForm.get('licenciaVigente').setValue( this.contratacionProyecto.licenciaVigente ? this.contratacionProyecto.licenciaVigente.toString() : false );    
            this.addressForm.get('numeroLicencia').setValue( this.contratacionProyecto.numeroLicencia );    
            this.addressForm.get('fechaVigencia').setValue( this.contratacionProyecto.fechaVigencia );
            this.addressForm.get('completada').setValue( this.contratacionProyecto.contempladaServicioMonitoreo ? this.contratacionProyecto.contempladaServicioMonitoreo.toString() : false ) ;    

            this.idSolicitud = this.contratacionProyecto.contratacionId;

            console.log( this.contratacionProyecto );

          }, 1000);
        })

        
      });
    }

    openDialog(modalTitle: string, modalText: string) {
      let dialogRef =this.dialog.open(ModalDialogComponent, {
        width: '28em',
        data: { modalTitle, modalText }
      });   
    }

  onSubmit() {

    this.contratacionProyecto.esReasignacion = this.addressForm.get('reasignacion').value;    
    this.contratacionProyecto.esAvanceObra = this.addressForm.get('avanceObra').value;    
    this.contratacionProyecto.porcentajeAvanceObra = this.addressForm.get('porcentajeAvanceObra').value;    
    this.contratacionProyecto.requiereLicencia = this.addressForm.get('requiereLicencias').value;    
    this.contratacionProyecto.licenciaVigente = this.addressForm.get('licenciaVigente').value;    
    this.contratacionProyecto.numeroLicencia = this.addressForm.get('numeroLicencia').value;    
    this.contratacionProyecto.fechaVigencia = this.addressForm.get('fechaVigencia').value;
    this.contratacionProyecto.contempladaServicioMonitoreo = this.addressForm.get('completada').value;  

    this.projectContractingService.createContratacionProyecto( this.contratacionProyecto )
      .subscribe( respuesta => {

        this.openDialog( "Solicitud Contratación", respuesta.message )

        if (respuesta.code == "200")
          this.router.navigate(["/solicitarContratacion/solicitud", this.contratacionProyecto.contratacionId ]);

      })

  }

  

}
