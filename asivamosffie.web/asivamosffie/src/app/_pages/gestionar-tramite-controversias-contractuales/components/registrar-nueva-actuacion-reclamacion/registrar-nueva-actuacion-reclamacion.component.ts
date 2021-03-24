import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';

@Component({
  selector: 'app-registrar-nueva-actuacion-reclamacion',
  templateUrl: './registrar-nueva-actuacion-reclamacion.component.html',
  styleUrls: ['./registrar-nueva-actuacion-reclamacion.component.scss']
})
export class RegistrarNuevaActuacionReclamacionComponent implements OnInit {
  controversiaId: any;
  reclamacionId: any;
  numReclamacion: any;
  //public numReclamacion = localStorage.getItem("numReclamacion");
  constructor(private activatedRoute: ActivatedRoute,private services: ContractualControversyService) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.controversiaId = param.idControversia;
      this.reclamacionId = param.idReclamacion;
      this.services.GetControversiaActuacionById(this.reclamacionId).subscribe((data:any)=>{
        this.numReclamacion = data.numeroActuacionReclamacion;
      });
    });
  }

}
